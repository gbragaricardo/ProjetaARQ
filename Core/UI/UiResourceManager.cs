using Microsoft.Extensions.DependencyInjection;
using ProjetaARQ.Core;
using System;
using System.Windows;
using Wpf.Ui.Appearance;
using Wpf.Ui.Markup;

namespace ProjetaARQ.Core.UI
{
    public static class UiResourceManager
    {
        private static bool _isInitialized = false;
        private static readonly Uri _designSystemUri = new Uri("pack://application:,,,/ProjetaARQ;component/Core/UI/DesignSystem.xaml", UriKind.Absolute);

        public static IThemeService ThemeService { get; private set; }

        /// <summary>
        /// Carrega e mescla o DesignSystem e os estilos do WPF-UI no processo ativo do Revit.
        /// </summary>
        public static void EnsureResources()
        {
            if (_isInitialized) return;

            try
            {
                // Se o ciclo de vida do WPF no processo do Revit ainda não possui uma Application ativa, criamos uma
                if (Application.Current == null)
                    new Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };

                // Dentro do EnsureResources()
                ThemeService = new ThemeService(_designSystemUri);
                var defaultTheme = ThemeService.GetCurrentTheme();

                // 2. Carrega o ThemesDictionary no escopo global com o tema inicial
                var themesDict = new ThemesDictionary { Theme = defaultTheme };
                Application.Current.Resources.MergedDictionaries.Add(themesDict);

                // 3. Carrega o ControlsDictionary globalmente para os estilos dos controles WPF-UI funcionarem
                var controlsDict = new ControlsDictionary();
                Application.Current.Resources.MergedDictionaries.Add(controlsDict);

                // 4. Carrega o dicionário do Design System
                var designSystemDict = new ResourceDictionary { Source = _designSystemUri };
                Application.Current.Resources.MergedDictionaries.Add(designSystemDict);

                // 5. Aplica as cores no motor do Wpf.Ui pela primeira vez
                ThemeService.ApplyProjetaAccents();

                _isInitialized = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro crítico ao inicializar o DesignSystem: {ex.Message}");
            }
        }
    }
}