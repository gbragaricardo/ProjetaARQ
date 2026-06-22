using System;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Markup;

namespace ProjetaARQ.Core.UI
{
    public static class UiResourceManager
    {
        private static bool _isInitialized = false;
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

                // 1. Carrega o ThemesDictionary no escopo global para que o ApplicationThemeManager consiga encontrá-lo
                var themesDict = new ThemesDictionary { Theme = ApplicationTheme.Light };
                Application.Current.Resources.MergedDictionaries.Add(themesDict);

                // 2. Carrega o ControlsDictionary globalmente para os estilos dos controles WPF-UI funcionarem
                var controlsDict = new ControlsDictionary();
                Application.Current.Resources.MergedDictionaries.Add(controlsDict);

                // 3. Carrega o dicionário do Design System
                var designSystemDict = new ResourceDictionary { Source = DesignSystemUri };

                // 4. Injeta o dicionário completo globalmente
                Application.Current.Resources.MergedDictionaries.Add(designSystemDict);

                // 3. Aplica as cores no motor do Wpf.Ui pela primeira vez
                ApplyProjetaAccents();

                _isInitialized = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro crítico ao inicializar o DesignSystem: {ex.Message}");
            }
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