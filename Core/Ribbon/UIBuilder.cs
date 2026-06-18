using Autodesk.Revit.UI;
using ProjetaARQ.Commands.DetailDoor;
using ProjetaARQ.Commands.FamiliesPane.Commands;
using ProjetaARQ.Commands.WordExport.Commands;

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
            _ribbonManager.AddPushButton(
                "FamiliesPaneButton",
                "Showroom\nBIM",
                typeof(FamiliesPaneButton).FullName,
                mainPanel,
                "ShowRoom de Famílias de Arquitetura",
                "catalogo.png",
                true);

            _ribbonManager.AddPushButton(
                "DevButton",
                "Memorial\nDescritivo",
                typeof(WordExport).FullName,
                mainPanel,
                "Em Desenvolvimento",
                "word.png",
                false);

            _ribbonManager.AddPushButton(
                "DetailDoorButton",
                "Montagem\nPortas",
                typeof(DetailDoorCommand).FullName,
                mainPanel,
                "Exportar Memorial Descritivo",
                "detail_door.png",
                true);


            #endregion

            #region DockablePane

            _dockablePaneManager.RegisterPanes();

            #endregion

        }
    }
}
