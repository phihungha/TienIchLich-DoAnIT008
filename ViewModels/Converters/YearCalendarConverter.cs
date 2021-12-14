using System;
using System.Globalization;
using System.Windows.Data;

namespace TienIchLich.ViewModels.Converters
{
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
