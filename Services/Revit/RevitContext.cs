using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace ProjetaARQ.Services.Revit
{
    internal class RevitContext
    {
        public UIApplication UiApp { get; private set; }
        public UIDocument UiDoc { get; private set; }
        public Application App { get; private set; }
        public Document Doc { get; private set; }
        public View ActiveView { get; private set; }

        public RevitContext(UIApplication uiApp)
        {
            UiApp = uiApp;
            UiDoc = uiApp.ActiveUIDocument;
            App = uiApp.Application;
            Doc = UiDoc?.Document;
            ActiveView = Doc.ActiveView;
        }
    }
}
