using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjetaARQ.Commands.DetailDoor.Services;
using ProjetaARQ.Core.DI;
using ProjetaARQ.Core.Services;
using ProjetaARQ.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Result = ProjetaARQ.Core.Results.Result;
namespace ProjetaARQ.Commands.DetailDoor
{
    public class DetailDoorHandler : ICommandHandler
    {
        private readonly IAppTelemetry _telemetry;
        private readonly IRevitContext _revitContext;
        private readonly IDoorService _doorService;
        private readonly IUIService<IDetailDoorResult> _uiService;

        public DetailDoorHandler(IAppTelemetry telemetry, IRevitContext revitContext, IDoorService doorService, IUIService<IDetailDoorResult> uIService)
        {
            _telemetry = telemetry;
            _revitContext = revitContext;
            _doorService = doorService;
            _uiService = uIService;
        }
        public Result Handle(ExternalCommandData commandData, ElementSet elements)
        {
            int createdViewsCount = 0;
            int commitedAssembliesCount = 0;

            _telemetry.LogInfo("Iniciando detalhamento de Portas...");

            if (commandData.Application.ActiveUIDocument == null)
                return Result.Warning("Nenhum projeto aberto.");
            

            IDetailDoorResult dialogResult = _uiService.ShowDialog();

            if (dialogResult == null)
                return Result.Cancelled();

            IList<Element> doorElements = _doorService.GetDoorsByCreatedPhaseId(_revitContext.Doc, dialogResult.PhaseItem?.Id).ToList();

            if (doorElements.Count == 0)
                return Result.Warning("Nenhuma porta com a fase selecionada foi encontrada no projeto.");

            HashSet<string> existingAssemblies = new FilteredElementCollector(_revitContext.Doc)
                .OfClass(typeof(AssemblyType))
                .Select(a => a.Name)
                .ToHashSet();

            HashSet<string> allFoundTypeMarks = new HashSet<string>();
            Dictionary<string, Element> doorsByTypeMark = new Dictionary<string, Element>();
            foreach (var door in doorElements)
            {
                Element doorType = _revitContext.Doc.GetElement(door.GetTypeId());

                Parameter typeMarkParameter = doorType?.get_Parameter(BuiltInParameter.WINDOW_TYPE_ID);

                if (typeMarkParameter == null || String.IsNullOrEmpty(typeMarkParameter.AsString()))
                    typeMarkParameter = doorType?.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_MARK);

                if (typeMarkParameter == null || String.IsNullOrEmpty(typeMarkParameter.AsString())) continue;

                string typeMark = typeMarkParameter.AsString();

                allFoundTypeMarks.Add(typeMark);

                if (existingAssemblies.Contains(typeMark)) continue;

                if (door.AssemblyInstanceId != ElementId.InvalidElementId) continue;

                if (doorsByTypeMark.ContainsKey(typeMark)) continue;

                doorsByTypeMark.Add(typeMark, door);
            }

            int ignoredDoorTypesCount = allFoundTypeMarks.Count - doorsByTypeMark.Count;

            using (TransactionGroup transGroup = new TransactionGroup(_revitContext.Doc, "Montagem Automática de Portas"))
            {
                transGroup.Start();

                foreach (var item in doorsByTypeMark)
                {
                    string typeMark = item.Key;
                    Element door = item.Value;
                    AssemblyInstance assemblyInstance = null;

                    using (Transaction tAssembly = new Transaction(_revitContext.Doc, $"Criar Montagem {typeMark}"))
                    {
                        tAssembly.Start();
                        try
                        {
                            List<ElementId> elementIds = new List<ElementId> { door.Id };
                            assemblyInstance = AssemblyInstance.Create(_revitContext.Doc, elementIds, door.Category.Id);
                            tAssembly.Commit();
                        }
                        catch (Exception ex)
                        {
                            _telemetry.LogError(ex, $"Erro ao criar montagem para a porta {typeMark}");
                            tAssembly.RollBack();
                            continue;
                        }
                    }

                    using (Transaction tRename = new Transaction(_revitContext.Doc, $"Renomear Montagem {typeMark}"))
                    {
                        tRename.Start();
                        SafeSetName(assemblyInstance, typeMark);
                        tRename.Commit();
                    }

                    foreach (var viewOption in dialogResult.ViewOptions)
                    {
                        using (Transaction tView = new Transaction(_revitContext.Doc, $"Criar Vista {typeMark}"))
                        {
                            tView.Start();
                            try
                            {
                                if (viewOption.IsViewOption3D == true)
                                {
                                    View3D threedView = AssemblyViewUtils.Create3DOrthographic(
                                        _revitContext.Doc,
                                        assemblyInstance.Id,
                                        viewOption.SelectedViewTemplate.Id,
                                        true);

                                    SafeSetName(threedView, $"{typeMark} - {viewOption.Tag}");
                                }
                                else
                                {
                                    ViewSection view = AssemblyViewUtils.CreateDetailSection(
                                        _revitContext.Doc,
                                        assemblyInstance.Id,
                                        viewOption.ViewOrientation,
                                        viewOption.SelectedViewTemplate.Id,
                                        true);

                                    SafeSetName(view, $"{typeMark} - {viewOption.Tag}");
                                }

                                tView.Commit();
                                createdViewsCount++;
                            }
                            catch (Exception ex)
                            {
                                _telemetry.LogError(ex, $"Erro ao criar vista para a montagem {typeMark}");
                                tView.RollBack();
                            }
                        }
                    }

                    commitedAssembliesCount++;
                }

                transGroup.Assimilate();
            }

            return Result.Success(
                $"Processo concluído!\n" +
                $"- {commitedAssembliesCount} Tipos de portas montados.\n" +
                $"- {createdViewsCount} Vistas geradas.\n" +
                $"- {ignoredDoorTypesCount} Tipos ignorados.");
        }

        private void SafeSetName(Element element, string baseName)
        {
            try
            {
                if (element is AssemblyInstance assembly)
                    assembly.AssemblyTypeName = baseName;

                else
                    element.Name = baseName;
            }
            catch
            {
                // Se o nome já existir, adiciona um sufixo aleatório para blindar o comando
                int suffix = new Random().Next(10, 99);
                string safeName = $"{baseName} ({suffix})";

                if (element is AssemblyInstance assembly)
                    assembly.AssemblyTypeName = safeName;
                else
                    element.Name = safeName;
            }
        }
    }
}
