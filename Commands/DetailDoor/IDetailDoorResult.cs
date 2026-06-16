using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetaARQ.Commands.DetailDoor
{
    public interface IDetailDoorResult
    {
        IList<ViewOptionItem> ViewOptions{ get; }
        ElementId PhaseId { get; }
    }
}
