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

        /// <summary>
        /// Instancia e adiciona um PushButton em uma RibbonPanel
        /// </summary>
        /// <param name="internName"></param>
        /// <param name="exhibitionName"></param>
        /// <param name="fullClassName"></param>
        /// <param name="panel"></param>
        /// <param name="buttonTip"></param>
        /// <param name="iconName"></param>
        /// <param name="enableOption"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Cria RibbonPanels na UI do Revit
        /// </summary>
        /// <param name="application"></param>
        /// <param name="tabName"></param>
        /// <param name="panelName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Cria Apenas os dados para instanciar um PushButton do Revit
        /// </summary>
        /// <param name="internName">Nome apenas no código</param>
        /// <param name="exhibitionName">Nome que aparece n UI do Revit</param>
        /// <param name="fullClassName">Nome da classe com namespaces</param>
        /// <param name="iconName">nome do icone na pasta Common/Icons</param>
        /// <param name="buttonTip">Dica de utilizacao que aparece na UI do Revit</param>
        /// <returns></returns>
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
