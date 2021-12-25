using System;
using System.Globalization;
using System.Windows.Data;

namespace TienIchLich.ViewModels.Converters
{
    [ValueConversion(typeof(CalendarEventStatusId), typeof(string))]
    public class EventStatusIdToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CalendarEventStatusId statusId;
            if (value != null && value is CalendarEventStatusId)
                statusId = (CalendarEventStatusId)value;
            else
                return "";

            if (statusId == CalendarEventStatusId.Upcoming)
                return "Tương lai";
            if (statusId == CalendarEventStatusId.Happening)
                return "Đang xảy ra";
            if (statusId == CalendarEventStatusId.Finished)
                return "Đã xong";

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
