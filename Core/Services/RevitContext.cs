using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ProjetaARQ.Core.Services
{
    public class RevitContext : IRevitContext
    {
        public UIApplication UIApp { get; private set; }
        public Application App => UIApp?.Application;
        public UIDocument UIDoc => UIApp?.ActiveUIDocument;
        public Document Doc => UIDoc?.Document;

        internal void Initialize(ExternalCommandData commandData)
        {
            UIApp = commandData.Application;
        }
    }
}
