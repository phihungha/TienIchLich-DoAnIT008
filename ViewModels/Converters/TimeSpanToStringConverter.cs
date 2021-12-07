using System;
using System.Globalization;
using System.Windows.Data;

namespace TienIchLich.ViewModels
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan timeSpan;
            if (value != null && value is TimeSpan)
                timeSpan = (TimeSpan)value;
            else
                return "";

            string timeSpanString = "";
            if (timeSpan.TotalDays >= 1)
                timeSpanString += $"{(int)timeSpan.TotalDays} ngày ";
            if (timeSpan.Hours != 0)
                timeSpanString += $"{timeSpan.Hours} tiếng ";
            if (timeSpan.Minutes != 0)
                timeSpanString += $"{timeSpan.Minutes} phút ";
            if (timeSpan.Seconds != 0)
                timeSpanString += $"{timeSpan.Seconds} giây ";
            return timeSpanString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
