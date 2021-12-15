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
            DateTime input = DateTime.Now;
            try
            {
                input = (DateTime)value;
                if (input != null)
                {
                    input = input.AddMonths(1);
                }
            }
            catch (Exception)
            {
            }
            return input;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime input = DateTime.Now;
            try
            {
                input = (DateTime)value;
                if (input != null)
                {
                    input = input.AddMonths(-1);
                }
            }
            catch (Exception)
            {
            }
            return input;
        }
    }
}
