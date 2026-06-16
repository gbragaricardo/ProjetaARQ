using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjetaARQ.Commands.DetailDoor.Services;
using ProjetaARQ.Commands.DetailDoor.ViewModels;
using ProjetaARQ.Commands.DetailDoor.Views;
using ProjetaARQ.Core.DI;
using ProjetaARQ.Core.Services;
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
        private readonly DetailDoorView _detailDoorView;
        private readonly DetailDoorViewModel _detailDoorViewModel;

        public DetailDoorHandler(IAppTelemetry telemetry, IRevitContext revitContext, IDoorService doorService, DetailDoorView detailDoorView, DetailDoorViewModel detailDoorViewModel)
        {
            _telemetry = telemetry;
            _revitContext = revitContext;
            _doorService = doorService;
            _detailDoorView = detailDoorView;
            _detailDoorViewModel = detailDoorViewModel;
        }
        public Result Handle(ExternalCommandData commandData, ElementSet elements)
        {
            _telemetry.LogInfo("Iniciando detalhamento de Portas...");

            if (commandData.Application.ActiveUIDocument == null)
                return Result.Failure("Nenhum projeto aberto.");

            if (_detailDoorView.ShowDialog() == false)
                return Result.Failure("Comando cancelado pelo usuário.");

            IDetailDoorResult dialogResult = _detailDoorViewModel;
            IList<Element> doorElements = _doorService.GetDoorsByCreatedPhaseId(_revitContext.Doc, dialogResult.PhaseId).ToList();

            if (doorElements.Count == 0)
                return Result.Failure($"Nenhuma porta com a fase selecionada foi encontrada no projeto.");

            Dictionary<string, Element> doorsByTypeMark = new Dictionary<string, Element>();
            foreach (var door in doorElements)
            {

                if (door.AssemblyInstanceId != ElementId.InvalidElementId)
                    continue;

                Element doorType = _revitContext.Doc.GetElement(door.GetTypeId());
                Parameter typeMarkParameter = doorType?.get_Parameter(BuiltInParameter.WINDOW_TYPE_ID);

                if (typeMarkParameter == null || String.IsNullOrEmpty(typeMarkParameter.AsString()))
                    typeMarkParameter = doorType?.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_MARK);

                if (typeMarkParameter == null || String.IsNullOrEmpty(typeMarkParameter.AsString())) 
                    continue;

                string typeMark = typeMarkParameter.AsString();

                if (doorsByTypeMark.ContainsKey(typeMark)) continue;

                doorsByTypeMark.Add(typeMark, door);
            }

            using (TransactionGroup transGroup = new TransactionGroup(_revitContext.Doc, "Montagem Automática de Portas"))
            {
                transGroup.Start();

                foreach (var item in doorsByTypeMark)
                {
                    string typeMark = item.Key;
                    Element door = item.Value;
                    AssemblyInstance assemblyInstance = null;

                    // 2. Transação isolada para criar a Montagem
                    using (Transaction tAssembly = new Transaction(_revitContext.Doc, $"Criar Montagem {typeMark}"))
                    {
                        tAssembly.Start();
                        try
                        {
                            List<ElementId> elementIds = new List<ElementId> { door.Id };
                            assemblyInstance = AssemblyInstance.Create(_revitContext.Doc, elementIds, door.Category.Id);

                            _revitContext.Doc.Regenerate(); // Essencial após criar a montagem
                            SafeSetName(assemblyInstance, typeMark);

                            tAssembly.Commit();
                        }
                        catch (Exception ex)
                        {
                            _telemetry.LogError(ex, $"Erro ao criar montagem para a porta {typeMark}");
                            tAssembly.RollBack();
                            continue; // Se a montagem falhou, pula para a próxima porta
                        }
                    }

                    // 3. Loop de Vistas (Cada vista tem sua própria transação)
                    foreach (var viewOption in dialogResult.ViewOptions)
                    {
                        using (Transaction tView = new Transaction(_revitContext.Doc, $"Criar Vista {typeMark}"))
                        {
                            tView.Start();
                            try
                            {
                                if (viewOption.IsViewOption3D == true)
                                {
                                    View3D threedView = AssemblyViewUtils.Create3DOrthographic(_revitContext.Doc, assemblyInstance.Id);
                                    threedView.ViewTemplateId = viewOption.SelectedViewTemplate.Id;
                                    SafeSetName(threedView, $"{typeMark} - 3D");
                                }
                                else
                                {
                                    ViewSection view = AssemblyViewUtils.CreateDetailSection(_revitContext.Doc, assemblyInstance.Id, viewOption.ViewOrientation);
                                    view.ViewTemplateId = viewOption.SelectedViewTemplate.Id;
                                    SafeSetName(view, $"{typeMark} - VOU MUDAR");
                                }

                                tView.Commit(); // Se deu certo, salva a vista
                            }
                            catch (Exception ex)
                            {
                                _telemetry.LogError(ex, $"Erro ao criar vista para a montagem {typeMark}");
                                tView.RollBack(); // Se essa vista falhou, desfaz APENAS essa vista, as outras continuam!
                            }
                        }
                    }
                }

                // 4. O Gran Finale: Amassa tudo em um único "Desfazer" na UI do Revit!
                transGroup.Assimilate();
            }

            return Result.Success();
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
