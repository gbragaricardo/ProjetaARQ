using Autodesk.Revit.UI;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Markup;

namespace ProjetaARQ.Core.UI
{
    public class ThemeService : IThemeService
    {

        private ApplicationTheme _currentTheme = ApplicationTheme.Light;

        public ThemeService()
        {
            // Inicializa o tema baseado no Revit assim que o serviço é construído
            _currentTheme = GetHostTheme();
        }

        public ApplicationTheme GetHostTheme()
        {
            try
            {
                if (UIThemeManager.CurrentTheme == UITheme.Dark)
                    return ApplicationTheme.Dark;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao detectar tema do Revit: {ex.Message}");
            }

            return ApplicationTheme.Light;
        }

        public void ApplyProjetaAccents(Uri designSystemUri)
        {
            try
            {
                var dict = new ResourceDictionary { Source = designSystemUri };

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

        public ApplicationTheme GetCurrentTheme() => _currentTheme;


        public void SetTheme(ApplicationTheme theme)
        {
            try
            {
                _currentTheme = theme;

                var themesDict = Application.Current?.Resources?.MergedDictionaries
                    .OfType<ThemesDictionary>()
                    .FirstOrDefault();

                if (themesDict != null)
                    themesDict.Theme = theme;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao definir tema global: {ex.Message}");
            }
        }
    }
}
