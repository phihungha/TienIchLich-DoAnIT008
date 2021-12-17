using System;
using System.Globalization;
using System.Windows.Data;

namespace TienIchLich.ViewModels.Converters
{
    /// <summary>
    /// Get month for the next month calendar on the year view using the month of the previous one.
    /// </summary>
    public class YearCalendarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is DateTime)
                return ((DateTime)value).AddMonths(1);
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
