using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using TienIchLich.ViewModels;

namespace TienIchLich.MonthEventCalendarControl
{
    /// <summary>
    /// Get events for the current CalendarDayButton.
    /// </summary>
    [ValueConversion(typeof(ObservableCollection<CalendarEventVM>), typeof(ObservableCollection<CalendarEventVM>))]
    public class CalendarEventConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime date = (DateTime)values[1];

            var thisDayEvents = new ObservableCollection<CalendarEventVM>();
            if (values[0] == null || !(values[0] is ObservableCollection<CalendarEventVM>))
                return thisDayEvents;

            foreach (CalendarEventVM calendarEvent in (ObservableCollection<CalendarEventVM>)values[0])
            {
                var startTime = calendarEvent.StartTime;
                var startDate = new DateTime(startTime.Year, startTime.Month, startTime.Day);
                if (startDate == date)
                    thisDayEvents.Add(calendarEvent);
            }

            return thisDayEvents;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
