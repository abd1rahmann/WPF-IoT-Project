using System;
using System.Globalization;
using System.Windows.Data;

namespace SimulatedFanApp.Converters
{
    public class BooleanToFanStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Om IsRunning är true, visa "Stoppa" eller en stoppikon; annars "Starta" eller en startikon
            return (bool)value ? "&#xf04d;" : "&#xf04b;"; // Stoppa-ikon och Starta-ikon i FontAwesome
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
