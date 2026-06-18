using System;
using System.Windows;
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

                var projetaRed = System.Windows.Media.Color.FromRgb(219, 15, 25);
                ApplicationAccentColorManager.Apply(projetaRed);


                var designSystemUri = new Uri("pack://application:,,,/ProjetaARQ;component/Core/UI/DesignSystem.xaml", UriKind.Absolute);
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = designSystemUri });


                _isInitialized = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro crítico ao inicializar o DesignSystem: {ex.Message}");
            }
        }
    }
}