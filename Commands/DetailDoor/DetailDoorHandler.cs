using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
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

        public DetailDoorHandler(IAppTelemetry telemetry, IRevitContext revitContext)
        {
            _telemetry = telemetry;
            _revitContext = revitContext;
        }
        public Result Handle(ExternalCommandData commandData, ElementSet elements)
        {
            _telemetry.LogInfo("Iniciando detalhamento de Portas...");

            if (commandData.Application.ActiveUIDocument == null)
                return Result.Failure("Nenhum projeto aberto.");

            var doorInstances = new FilteredElementCollector(_revitContext.Doc)
                .OfCategory(BuiltInCategory.OST_Doors)
                .WhereElementIsNotElementType()
                .ToElements();

            if (doorInstances.Count == 0)
                return Result.Failure("Nenhuma porta encontrada no projeto.");

            Dictionary<Element, string> doorsByPhase = new Dictionary<Element, string>();

            foreach (var door in doorInstances)
            {
                Parameter phaseParameter = door.get_Parameter(BuiltInParameter.PHASE_CREATED);

                if (phaseParameter == null) continue;

                string phaseValue = phaseParameter.AsValueString();

                doorsByPhase.Add(door, phaseValue);
            }


            if (doorsByPhase.Count == 0)
                return Result.Failure("As Portas não possuem valor no parâmetro 'Fase Criada'");

            string choosenPhase = string.Empty; // Aqui virá de um input do usuário

            var phaseFilteredDoors = doorsByPhase
                .Where(d => d.Value == choosenPhase)
                .Select(d => d.Key)
                .ToList();

            Dictionary<string, Element> doorsByTypeMark = new Dictionary<string, Element>();

            foreach (var door in phaseFilteredDoors) 
            {
                Parameter typeMarkParameter = door.get_Parameter(BuiltInParameter.WINDOW_TYPE_ID);

                if (typeMarkParameter == null) continue;

                string typeMark = typeMarkParameter.AsValueString();

                if (doorsByTypeMark.ContainsKey(typeMark)) continue;

                doorsByTypeMark.Add(typeMark, door);
            }

            // --- PASSO 7: Iniciar a Transação e a Criação das Montagens ---
            using (Transaction trans = new Transaction(_revitContext.Doc, "Detalhamento Automático Portas"))
            {
                trans.Start();

                int count = 0;

                foreach (var item in doorsByTypeMark)
                {
                    string typeMark = item.Key;
                    Element door = item.Value;

                    // Verifica se a porta já não está em uma montagem para não duplicar
                    if (door.AssemblyInstanceId != ElementId.InvalidElementId)
                    {
                        _telemetry.LogInfo($"A porta {typeMark} já possui montagem. Pulando...");
                        continue;
                    }

                    // A API do Revit exige uma lista de IDs, mesmo que seja para um único elemento
                    List<ElementId> elementIds = new List<ElementId> { door.Id };

                    AssemblyInstance assemblyInstance = null;
                    try
                    {
                        // Cria a montagem na categoria de Portas
                        assemblyInstance = AssemblyInstance.Create(_revitContext.Doc, elementIds, door.Category.Id);
                    }
                    catch (Exception ex)
                    {
                        _telemetry.LogError(ex, $"Erro ao criar montagem para a porta {typeMark}");
                        continue;
                    }

                    // Força o Revit a processar a criação da montagem na memória antes de nomear
                    _revitContext.Doc.Regenerate();

                    // Nomeia a montagem com a Marca de Tipo (ex: P1, P2)
                    // Criamos aquele método auxiliar 'SafeSetName' que resolve conflito de nome se já existir
                    SafeSetName(assemblyInstance, typeMark);

                    try
                    {
                        // --- CRIAÇÃO DAS VISTAS DA MONTAGEM ---

                        // 1. Planta (Horizontal)
                        ViewSection planView = AssemblyViewUtils.CreateDetailSection(_revitContext.Doc, assemblyInstance.Id, AssemblyDetailViewOrientation.HorizontalDetail);
                        SafeSetName(planView, $"{typeMark} - PLANTA");
                         //if (planTemplate != null) planView.ViewTemplateId = planTemplate.Id;

                        // 2. Vista Frontal (Elevação)
                        ViewSection frontView = AssemblyViewUtils.CreateDetailSection(_revitContext.Doc, assemblyInstance.Id, AssemblyDetailViewOrientation.ElevationFront);
                        SafeSetName(frontView, $"{typeMark} - VISTA FRONTAL");
                        

                        // 3. Corte (Perfil)
                        ViewSection sectionView = AssemblyViewUtils.CreateDetailSection(_revitContext.Doc, assemblyInstance.Id, AssemblyDetailViewOrientation.DetailSectionA);
                        SafeSetName(sectionView, $"{typeMark} - CORTE");

                        // 4. Vista 3D Isométrica
                        View3D threedView = AssemblyViewUtils.Create3DOrthographic(_revitContext.Doc, assemblyInstance.Id);
                        SafeSetName(threedView, $"{typeMark} - 3D");

                        count++;
                    }
                    catch (Exception ex)
                    {
                        _telemetry.LogError(ex, $"Erro ao criar vistas da montagem {typeMark}");
                    }
                }

                trans.Commit();
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
