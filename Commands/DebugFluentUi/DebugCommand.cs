using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjetaARQ.Core.Commands;

namespace ProjetaARQ.Commands.DebugFluentUi
{
    [Transaction(TransactionMode.Manual)]
    public class DebugCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return CommandRunner.Run<DebugHandler>(commandData, ref message, elements);
        }
    }
}
