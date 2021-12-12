using System;
using System.Globalization;
using System.Windows.Data;

namespace TienIchLich.ViewModels.Converters
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeSpanToString : IValueConverter
    {
        public static string ConvertFromTimeSpan(TimeSpan timeSpan)
        {
            string timeSpanString = "";
            if (timeSpan.TotalDays >= 1)
                timeSpanString += $"{(int)timeSpan.TotalDays} ngày ";
            if (timeSpan.Hours != 0)
                timeSpanString += $"{timeSpan.Hours} tiếng ";
            if (timeSpan.Minutes != 0)
                timeSpanString += $"{timeSpan.Minutes} phút ";
            if (timeSpan.Seconds != 0)
                timeSpanString += $"{timeSpan.Seconds} giây ";
            if (timeSpan.TotalSeconds == 0)
                timeSpanString = "Ngay lập tức";
            return timeSpanString;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan timeSpan;
            if (value != null && value is TimeSpan)
                timeSpan = (TimeSpan)value;
            else
                return "";
            return ConvertFromTimeSpan(timeSpan);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
