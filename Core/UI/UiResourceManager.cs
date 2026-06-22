using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Markup;

namespace ProjetaARQ.Core.UI
{
    public static class UiResourceManager
    {
        private static bool _isInitialized = false;
        private static ApplicationTheme _currentTheme = ApplicationTheme.Light;
        private static readonly Uri DesignSystemUri = new Uri("pack://application:,,,/ProjetaARQ;component/Core/UI/DesignSystem.xaml", UriKind.Absolute);

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

                // 1. Detecta o tema ativo do Revit para usar como padrão
                var defaultTheme = GetRevitTheme();
                _currentTheme = defaultTheme;

                // 2. Carrega o ThemesDictionary no escopo global com o tema inicial
                var themesDict = new ThemesDictionary { Theme = defaultTheme };
                Application.Current.Resources.MergedDictionaries.Add(themesDict);

                // 3. Carrega o ControlsDictionary globalmente para os estilos dos controles WPF-UI funcionarem
                var controlsDict = new ControlsDictionary();
                Application.Current.Resources.MergedDictionaries.Add(controlsDict);

                // 4. Carrega o dicionário do Design System
                var designSystemDict = new ResourceDictionary { Source = DesignSystemUri };
                Application.Current.Resources.MergedDictionaries.Add(designSystemDict);

                // 5. Aplica as cores no motor do Wpf.Ui pela primeira vez
                ApplyProjetaAccents();

                _isInitialized = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro crítico ao inicializar o DesignSystem: {ex.Message}");
            }
        }

        public static ApplicationTheme GetRevitTheme()
        {
            try
            {
                // Verifica o tema do Revit através do UIThemeManager (disponível a partir do Revit 2024)
                if (Autodesk.Revit.UI.UIThemeManager.CurrentTheme == Autodesk.Revit.UI.UITheme.Dark)
                {
                    return ApplicationTheme.Dark;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao detectar tema do Revit: {ex.Message}");
            }
            return ApplicationTheme.Light;
        }

        public static void SetTheme(ApplicationTheme theme)
        {
            try
            {
                _currentTheme = theme;
                var themesDict = Application.Current?.Resources?.MergedDictionaries
                    .OfType<ThemesDictionary>()
                    .FirstOrDefault();
                if (themesDict != null)
                {
                    themesDict.Theme = theme;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao definir tema global: {ex.Message}");
            }
        }

        public static ApplicationTheme GetCurrentTheme()
        {
            return _currentTheme;
        }

        public static void ApplyProjetaAccents()
        {
            try
            {
                var dict = new ResourceDictionary { Source = DesignSystemUri };
                var primaryColor = (Color)dict["SystemAccentColorPrimary"];
                var secondaryColor = (Color)dict["SystemAccentColorSecondary"];
                var tertiaryColor = (Color)dict["SystemAccentColorTertiary"];

                ApplicationAccentColorManager.Apply(
                    systemAccent: primaryColor,
                    primaryAccent: primaryColor,
                    secondaryAccent: secondaryColor,
                    tertiaryAccent: tertiaryColor
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Falha ao injetar acentos: {ex.Message}");
            }
        }
    }
}