using System;
using System.Windows;

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

                // URI ajustada perfeitamente para ler o arquivo na pasta Core/UI/
                var designSystemUri = new Uri("pack://application:,,,/ProjetaARQ;component/Core/UI/DesignSystem.xaml", UriKind.Absolute);
                var projetaResources = new ResourceDictionary { Source = designSystemUri };

                // Mescla os estilos de forma que fiquem disponíveis globalmente na DLL
                Application.Current.Resources.MergedDictionaries.Add(projetaResources);

                _isInitialized = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro crítico ao inicializar o DesignSystem: {ex.Message}");
            }
        }
    }
}