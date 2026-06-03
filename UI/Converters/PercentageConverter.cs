using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ProjetaARQ.UI.Core.Converters
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double parentSize/* && !double.IsNaN(parentSize) && !double.IsInfinity(parentSize)*/)
            {
                if (double.TryParse(parameter as string, NumberStyles.Any, CultureInfo.InvariantCulture, out double percentage))
                    return parentSize * (percentage / 100.0);             
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
