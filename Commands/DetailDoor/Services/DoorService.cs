using Autodesk.Revit.DB;
using ProjetaARQ.Commands.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetaARQ.Commands.DetailDoor.Services
{
    public class DoorService : IDoorService
    {
        public IEnumerable<Element> GetDoorsByCreatedPhaseId(Document doc, ElementId phaseId)
        {
            return new FilteredElementCollector(doc)
                 .OfCategory(BuiltInCategory.OST_Doors)
                 .WhereElementIsNotElementType()
                 .Where(e => e.CreatedPhaseId == phaseId);
        }

        public IEnumerable<RevitPhaseItem> GetPhasesFromDoors(Document doc)
        {
            var doors= new FilteredElementCollector(doc)
                            .OfCategory(BuiltInCategory.OST_Doors)
                            .WhereElementIsNotElementType()
                            .ToList();

            return doors
                .Select(d => d.CreatedPhaseId)
                .Where(phaseId => phaseId != ElementId.InvalidElementId)
                .Distinct()
                .Select(phaseId =>
                {
                    var phase = doc.GetElement(phaseId) as Phase;
                    return new RevitPhaseItem(phase?.Name ?? "Fase não encontrada", phaseId);
                });
        }
    }
}
