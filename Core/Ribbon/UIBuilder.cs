using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using ProjetaARQ.Core.Ribbon.Interfaces;

namespace ProjetaARQ.Core.Ribbon
{
    internal class UIBuilder
    {
        private readonly string _tabName = "ProjetaARQ";

        private readonly UIControlledApplication _app;
        private readonly IRibbonManager _ribbonManager;
        private readonly IDockablePaneManager _dockablePaneManager;

        public UIBuilder(UIControlledApplication app, IRibbonManager ribbonManager, IDockablePaneManager DockablePaneManager)
        {
            _app = app;
            _ribbonManager = ribbonManager;
            _dockablePaneManager = DockablePaneManager;
        }

        /// <summary>
        /// Cria os elementos na UI do revit, criando os itens e alocando na ordem
        /// </summary>
        public void Build()
        {
            #region Tab

            _app.CreateRibbonTab(_tabName);

            #endregion

            #region Panels

            RibbonPanel mainPanel = _ribbonManager.CreatePanel(_app, _tabName, "Arquitetura");

            #endregion

            #region Buttons
            var familiesPaneButton = _ribbonManager.AddPushButton(
                "FamiliesPaneButton",
                "Showroom\nBIM",
                "ProjetaARQ.Commands.FamiliesPane.Commands.FamiliesPaneButton",
                mainPanel,
                "ShowRoom de Famílias de Arquitetura",
                "catalogo.png",
                true);

            var testButton = _ribbonManager.AddPushButton(
                "DevButton",
                "Memorial\nDescritivo",
                "ProjetaARQ.Commands.WordExport.Commands.WordExport",
                mainPanel,
                "Em Desenvolvimento",
                "word.png",
                false);


            #endregion

            #region DockablePane

            _dockablePaneManager.RegisterPanes();

            #endregion

        }
    }
}
