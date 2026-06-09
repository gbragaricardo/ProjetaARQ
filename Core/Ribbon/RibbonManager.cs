using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using ProjetaARQ.Services;

namespace ProjetaARQ.Core.Ribbon
{
    internal class RibbonManager : IRibbonManager
    {
        private readonly string _thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
        public PushButton AddPushButton(
            string internName,
            string exhibitionName,
            string fullClassName,
            RibbonPanel panel,
            string buttonTip,
            string iconName,
            bool enableOption = false)
        {
            PushButtonData pushButtonData = CreatePushButtonData(internName, exhibitionName, fullClassName, iconName, buttonTip);

            if (panel == null || pushButtonData == null)
                return null;
            
            PushButton pushButton = panel.AddItem(pushButtonData) as PushButton;
            pushButton.Enabled = enableOption;
            return pushButton;
        }

        public RibbonPanel CreatePanel(UIControlledApplication application, string tabName, string panelName)
        {
            try 
            { 
                return application.CreateRibbonPanel(tabName, panelName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao criar o painel '{panelName}': {ex.Message}");
                return null;
            }
        }

        public PushButtonData CreatePushButtonData(string internName, string exhibitionName, string fullClassName, string iconName, string buttonTip)
        {
            var pushButtonData = new PushButtonData(internName, exhibitionName, _thisAssemblyPath, fullClassName);

            BitmapImage bitmap = BitmapConverter.GetIcon(iconName);
            pushButtonData.LargeImage = bitmap;
            //pushButtonData.Image = bitmap;
            pushButtonData.ToolTip = buttonTip;

            return pushButtonData;
        }
    }
}
