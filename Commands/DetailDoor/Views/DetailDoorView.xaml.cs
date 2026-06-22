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
        private ApplicationTheme _currentTheme = ApplicationTheme.Light;

        // O construtor recebe a ViewModel via Injeção de Dependência do nosso contêiner Scoped
        public DetailDoorView(DetailDoorViewModel viewModel)
        {
            InitializeComponent();

            // Define o DataContext para o MVVM funcionar de forma limpa
            DataContext = viewModel;
            viewModel.RequestClose += () => this.DialogResult = true;

            // Inicializa de acordo com o tema global do Revit/aplicação em memória
            _currentTheme = UiResourceManager.GetCurrentTheme();
            UpdateThemeIcon();
        }

        private void OnToggleThemeClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Alterna o estado local do tema
                _currentTheme = _currentTheme == ApplicationTheme.Light
                    ? ApplicationTheme.Dark
                    : ApplicationTheme.Light;

                // 2. Aplica o tema globalmente no nível da aplicação em memória
                UiResourceManager.SetTheme(_currentTheme);

                // 3. Atualiza o ícone da lua/sol correspondente
                UpdateThemeIcon();

                // 4. Reaplica as cores de acento do vermelho Projeta
                UiResourceManager.ApplyProjetaAccents();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao alternar o tema global: {ex.Message}");
            }
        }

        private void UpdateThemeIcon()
        {
            ThemeIcon.Symbol = _currentTheme == ApplicationTheme.Light
                ? SymbolRegular.WeatherMoon24
                : SymbolRegular.WeatherSunny24;
        }
    }
}
