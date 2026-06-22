using System;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Appearance;

namespace ProjetaARQ.Core.UI
{
    public static class UiResourceManager
    {
        private static bool _isInitialized = false;

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
                {
                    new Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
                }

                // 1. Carrega a base do WPF-UI (Tema Claro e Controles)
                var lightThemeUri = new Uri("pack://application:,,,/Wpf.Ui;component/Resources/Theme/Light.xaml", UriKind.Absolute);
                var wpfUiUri = new Uri("pack://application:,,,/Wpf.Ui;component/Resources/Wpf.Ui.xaml", UriKind.Absolute);

                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = lightThemeUri });
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = wpfUiUri });

                var designSystemUri = new Uri("pack://application:,,,/ProjetaARQ;component/Core/UI/DesignSystem.xaml", UriKind.Absolute);
                var designSystemDict = new ResourceDictionary { Source = designSystemUri };

                // 2. Extrai as cores exatas que você definiu no XAML
                var primaryColor = (Color)designSystemDict["SystemAccentColorPrimary"];
                var secondaryColor = (Color)designSystemDict["SystemAccentColorSecondary"];
                var tertiaryColor = (Color)designSystemDict["SystemAccentColorTertiary"];

                // 3. Alimenta o motor do Wpf.Ui com as cores da Projeta para gerar o Hover/Pressed perfeitos
                ApplicationAccentColorManager.Apply(
                    systemAccent: primaryColor,
                    primaryAccent: primaryColor,
                    secondaryAccent: secondaryColor,
                    tertiaryAccent: tertiaryColor
                );

                // 4. Injeta o dicionário completo no processo do Revit (garantindo que seus Brushes sobrescrevam qualquer padrão)
                Application.Current.Resources.MergedDictionaries.Add(designSystemDict);

                _isInitialized = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro crítico ao inicializar o DesignSystem: {ex.Message}");
            }
        }
    }
}