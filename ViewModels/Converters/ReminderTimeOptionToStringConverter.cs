using System;
using System.Globalization;
using System.Windows.Data;

namespace TienIchLich.ViewModels
{
    [ValueConversion(typeof(ReminderTimeOption), typeof(string))]
    class ReminderTimeOptionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ReminderTimeOption option;
            try
            {
                option = (ReminderTimeOption)value;
            }
            catch (InvalidCastException)
            {
                return "";
            }

            switch (option)
            {
                case ReminderTimeOption.Immediately:
                    return "Ngay lập tức";
                case ReminderTimeOption.Minutes5:
                    return "5 phút";
                case ReminderTimeOption.Minutes15:
                    return "15 phút";
                case ReminderTimeOption.Minutes30:
                    return "30 phút";
                case ReminderTimeOption.Hour1:
                    return "1 tiếng";
                case ReminderTimeOption.Hour12:
                    return "12 tiếng";
                case ReminderTimeOption.Day1:
                    return "1 ngày";
                case ReminderTimeOption.Week1:
                    return "1 tháng";
                case ReminderTimeOption.Custom:
                    return "Tùy chọn";
            }
            return option.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
