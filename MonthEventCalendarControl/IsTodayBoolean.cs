using System;
using System.Globalization;
using System.Windows.Data;

namespace TienIchLich.MonthEventCalendarControl
{
    /// <summary>
    /// If the current day is today.
    /// </summary>
    public class IsTodayBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null && !(value is DateTime))
                return false;

            if (((DateTime)value).Date == DateTime.Now.Date)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}