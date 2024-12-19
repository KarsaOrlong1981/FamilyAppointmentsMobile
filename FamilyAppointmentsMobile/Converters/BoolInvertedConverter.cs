using System.Globalization;

namespace FamilyAppointmentsMobile.Converters
{
    public sealed class BoolInvertedConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool boolValue && !boolValue;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool boolValue && !boolValue;
        }
    }
}
