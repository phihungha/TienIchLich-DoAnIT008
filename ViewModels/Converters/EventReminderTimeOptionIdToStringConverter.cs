using System;
using System.Globalization;
using System.Windows.Data;

namespace TienIchLich.ViewModels
{
    [ValueConversion(typeof(EventReminderTimeOptionId), typeof(string))]
    class EventReminderTimeOptionIdToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EventReminderTimeOptionId option;
            try
            {
                option = (EventReminderTimeOptionId)value;
            }
            catch (InvalidCastException)
            {
                return "";
            }

            switch (option)
            {
                case EventReminderTimeOptionId.Immediately:
                    return "Ngay lập tức";
                case EventReminderTimeOptionId.Minutes5:
                    return "5 phút";
                case EventReminderTimeOptionId.Minutes15:
                    return "15 phút";
                case EventReminderTimeOptionId.Minutes30:
                    return "30 phút";
                case EventReminderTimeOptionId.Hour1:
                    return "1 tiếng";
                case EventReminderTimeOptionId.Hour12:
                    return "12 tiếng";
                case EventReminderTimeOptionId.Day1:
                    return "1 ngày";
                case EventReminderTimeOptionId.Week1:
                    return "1 tháng";
                case EventReminderTimeOptionId.Custom:
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
