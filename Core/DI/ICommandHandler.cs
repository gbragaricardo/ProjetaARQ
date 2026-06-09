using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Result = ProjetaARQ.Core.Results.Result;


namespace ProjetaARQ.Core.DI
{
    public interface ICommandHandler
    {
        // Repare que ele usa o SEU Result, não o do Revit
        Result Handle(ExternalCommandData commandData, ElementSet elements);
    }
}
