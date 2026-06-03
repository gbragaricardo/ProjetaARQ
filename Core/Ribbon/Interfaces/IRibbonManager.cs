using Autodesk.Revit.UI;

namespace ProjetaARQ.Core.Ribbon.Interfaces
{
    public interface IRibbonManager
    {
        RibbonPanel CreatePanel(UIControlledApplication application, string tabName, string panelName);

        PushButton AddPushButton(
            string internName,
            string exhibitionName,
            string fullClassName,
            RibbonPanel panel,
            string buttonTip,
            string iconName,
            bool enableOption = false);
    }
}
