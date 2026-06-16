using Autodesk.Revit.DB;
using ProjetaARQ.Commands.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetaARQ.Commands.DetailDoor.Services
{
    public interface IDoorService
    {
        IEnumerable<RevitPhaseItem> GetPhasesFromDoors(Document doc);
        IEnumerable<Element> GetDoorsByCreatedPhaseId(Document doc, ElementId phaseId);

    }
}
