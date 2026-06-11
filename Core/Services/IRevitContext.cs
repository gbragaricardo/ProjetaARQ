using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ProjetaARQ.Core.Services
{
    public interface IRevitContext
    {
        UIApplication UIApp { get; }
        Application App { get; }
        UIDocument UIDoc { get; }
        Document Doc { get; }
    }
}
