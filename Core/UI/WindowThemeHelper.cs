using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace ProjetaARQ.Core.UI
{
    public static class WindowThemeHelper
    {
        public static void InitializeTheme(SymbolIcon themeIcon, IThemeService themeService)
        {
            var theme = themeService.GetCurrentTheme();
            UpdateIcon(themeIcon, theme);
        }
        public static ApplicationTheme ToggleTheme(SymbolIcon themeIcon, IThemeService themeService)
        {
            var current = themeService.GetCurrentTheme();
            var next = current == ApplicationTheme.Light ? ApplicationTheme.Dark : ApplicationTheme.Light;

            themeService.SetTheme(next);
            UpdateIcon(themeIcon, next);
            themeService.ApplyProjetaAccents();

            return next;
        }
        private static void UpdateIcon(SymbolIcon themeIcon, ApplicationTheme theme)
        {
            if (themeIcon != null)
            {
                themeIcon.Symbol = theme == ApplicationTheme.Light
                    ? SymbolRegular.WeatherMoon24
                    : SymbolRegular.WeatherSunny24;
            }
        }
    }
}
