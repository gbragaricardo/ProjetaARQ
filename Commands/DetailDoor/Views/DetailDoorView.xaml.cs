using ProjetaARQ.Commands.DetailDoor.ViewModels;
using ProjetaARQ.Core.UI;
using System.Windows;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace ProjetaARQ.Commands.DetailDoor.Views
{
    public partial class DetailDoorView : FluentWindow
    {
        // O construtor recebe a ViewModel via Injeção de Dependência do nosso contêiner Scoped
        public DetailDoorView(DetailDoorViewModel viewModel)
        {
            InitializeComponent();

            // Define o DataContext para o MVVM funcionar de forma limpa
            DataContext = viewModel;
            viewModel.RequestClose += () => this.DialogResult = true;
        }

        private void OnToggleThemeClicked(object sender, RoutedEventArgs e)
        {
            // 1. Pega o tema atual
            var currentTheme = ApplicationThemeManager.GetAppTheme();
            var newTheme = currentTheme == ApplicationTheme.Light
                ? ApplicationTheme.Dark
                : ApplicationTheme.Light;

            // 2. Troca o tema (O Lepo agora vai conseguir achar a âncora no XAML e mudar tudo)
            ApplicationThemeManager.Apply(newTheme);
            ApplicationThemeManager.Apply(this);

            // 3. Atropela o reset do Lepo e devolve o Vermelho Projeta para a interface
            UiResourceManager.ApplyProjetaAccents();

            // 4. Atualiza o ícone da lua/sol
            ThemeIcon.Symbol = newTheme == ApplicationTheme.Light
                ? SymbolRegular.WeatherMoon24
                : SymbolRegular.WeatherSunny24;
        }
    }
}
