using ProjetaARQ.Commands.DetailDoor.ViewModels;
using ProjetaARQ.Core.UI;
using System;
using System.Linq;
using System.Windows;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace ProjetaARQ.Commands.DetailDoor.Views
{
    public partial class DetailDoorView : FluentWindow
    {
        private readonly IThemeService _themeService;

        public DetailDoorView(DetailDoorViewModel viewModel, IThemeService themeService)
        {
            InitializeComponent();

            _themeService = themeService;
            DataContext = viewModel;

            viewModel.RequestClose += () => this.DialogResult = true;

            WindowThemeHelper.InitializeTheme(ThemeIcon, _themeService);
        }

        private void OnToggleThemeClicked(object sender, RoutedEventArgs e)
        {
            WindowThemeHelper.ToggleTheme(ThemeIcon, _themeService);
        }
    }
}
