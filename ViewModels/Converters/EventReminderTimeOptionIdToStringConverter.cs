using System;
using System.Globalization;
using System.Windows.Data;

namespace TienIchLich.ViewModels
{
    [ValueConversion(typeof(CalendarEventReminderTimeOptionId), typeof(string))]
    class EventReminderTimeOptionIdToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CalendarEventReminderTimeOptionId option;
            try
            {
                option = (CalendarEventReminderTimeOptionId)value;
            }
            catch (InvalidCastException)
            {
                return "";
            }

            switch (option)
            {
                case CalendarEventReminderTimeOptionId.Immediately:
                    return "Ngay lập tức";
                case CalendarEventReminderTimeOptionId.Minutes5:
                    return "5 phút";
                case CalendarEventReminderTimeOptionId.Minutes15:
                    return "15 phút";
                case CalendarEventReminderTimeOptionId.Minutes30:
                    return "30 phút";
                case CalendarEventReminderTimeOptionId.Hour1:
                    return "1 tiếng";
                case CalendarEventReminderTimeOptionId.Hour12:
                    return "12 tiếng";
                case CalendarEventReminderTimeOptionId.Day1:
                    return "1 ngày";
                case CalendarEventReminderTimeOptionId.Week1:
                    return "1 tháng";
                case CalendarEventReminderTimeOptionId.Custom:
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
