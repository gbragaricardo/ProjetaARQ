using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Appearance;


namespace ProjetaARQ.Core.UI
{
    public interface IThemeService
    {
        ApplicationTheme GetHostTheme();
        ApplicationTheme GetCurrentTheme();
        void SetTheme(ApplicationTheme theme);
        void ApplyProjetaAccents(Uri designSystemUri);
    }
}
