using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using TienIchLich.ViewModels;

namespace TienIchLich.MonthEventCalendarControl
{
    /// <summary>
    /// Get calendar events for the current CalendarDayButton.
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

            foreach (CalendarEventVM calendarEventVM in (ObservableCollection<CalendarEventVM>)values[0])
            {
                var startTime = calendarEventVM.StartTime;
                var startDate = new DateTime(startTime.Year, startTime.Month, startTime.Day);
                bool isDisplayed = calendarEventVM.CategoryVM.IsDisplayed;
                if (startDate == date && isDisplayed)
                    thisDayEvents.Add(calendarEventVM);
            }

            return thisDayEvents;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
