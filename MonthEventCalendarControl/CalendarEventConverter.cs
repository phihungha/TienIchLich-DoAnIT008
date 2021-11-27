using System;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace TienIchLich.MonthEventCalendarControl
{
    /// <summary>
    /// Get events for the current CalendarDayButton.
    /// </summary>
    [ValueConversion(typeof(ObservableCollection<CalendarEventViewModel>), typeof(ObservableCollection<CalendarEventViewModel>))]
    public class CalendarEventConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime date = (DateTime)values[1];

            var thisDayEvents = new ObservableCollection<CalendarEventViewModel>();
            foreach (CalendarEventViewModel calendarEvent in (ObservableCollection<CalendarEventViewModel>)values[0])
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
