using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProjetaARQ.UI.Core.Converters
{
    public sealed class BoolToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return boolValue ? Visibility.Visible : Visibility.Collapsed;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            =>  Binding.DoNothing; // Não implementado, pois não é necessário converter de volta
        
    }
}
