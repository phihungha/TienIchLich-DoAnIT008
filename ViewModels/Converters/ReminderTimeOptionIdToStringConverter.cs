using System;
using System.Globalization;
using System.Windows.Data;

namespace TienIchLich.ViewModels
{
    [ValueConversion(typeof(ReminderTimeOptionId), typeof(string))]
    class ReminderTimeOptionIdToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ReminderTimeOptionId option;
            try
            {
                option = (ReminderTimeOptionId)value;
            }
            catch (InvalidCastException)
            {
                return "";
            }

            switch (option)
            {
                case ReminderTimeOptionId.Immediately:
                    return "Ngay lập tức";
                case ReminderTimeOptionId.Minutes5:
                    return "5 phút";
                case ReminderTimeOptionId.Minutes15:
                    return "15 phút";
                case ReminderTimeOptionId.Minutes30:
                    return "30 phút";
                case ReminderTimeOptionId.Hour1:
                    return "1 tiếng";
                case ReminderTimeOptionId.Hour12:
                    return "12 tiếng";
                case ReminderTimeOptionId.Day1:
                    return "1 ngày";
                case ReminderTimeOptionId.Week1:
                    return "1 tháng";
                case ReminderTimeOptionId.Custom:
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
