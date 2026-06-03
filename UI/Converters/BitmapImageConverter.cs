using ProjetaARQ.Services;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ProjetaARQ.UI.Core.Converters
{
    public class BitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (parameter is string imageName)
                {
                    return BitmapConverter.GetResource(imageName);
                }
                else if (value is string imageBindName)
                {
                    return BitmapConverter.GetResource(imageBindName);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
