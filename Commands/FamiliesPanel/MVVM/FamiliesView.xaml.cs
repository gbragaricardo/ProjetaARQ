using Autodesk.Revit.UI;
using ControlzEx.Theming;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;

namespace ProjetaARQ.Commands.FamiliesPanel.MVVM
{
    /// <summary>
    /// Interaction logic for FamiliesView.xaml
    /// </summary>
    public partial class FamiliesView : Page, IDockablePaneProvider
    {
        internal FamiliesView(FamiliesViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.VisibleByDefault = false;
            data.FrameworkElement = (FrameworkElement)this; // Define a UI que será exibida no painel
            data.InitialState = new DockablePaneState
            {
                DockPosition = DockPosition.Right, // Define a posição do painel
                MinimumWidth = 400
            };
        }
    }
}
