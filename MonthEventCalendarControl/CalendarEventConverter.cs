using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using TienIchLich.ViewModels;

namespace TienIchLich.MonthEventCalendarControl
{
    /// <summary>
    /// Get calendar events for the current CalendarDayButton.
    /// </summary>
    [ValueConversion(typeof(Dictionary<DateTime, ObservableCollection<CalendarEventCardVM>>), typeof(ObservableCollection<CalendarEventVM>))]
    public class CalendarEventConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[1] == null || !(values[1] is DateTime))
                return null;

            var currentDate = (DateTime)values[1];

            if (values[0] == null || !(values[0] is Dictionary<DateTime, ObservableCollection<CalendarEventCardVM>>))
                return null;

            var eventCardVMs = (Dictionary<DateTime, ObservableCollection<CalendarEventCardVM>>)values[0];
            if (eventCardVMs.ContainsKey(currentDate))
                return eventCardVMs[currentDate];
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Get whether there is event in current day.
    /// </summary>
    [ValueConversion(typeof(Dictionary<DateTime, ObservableCollection<CalendarEventCardVM>>), typeof(bool))]
    public class CalendarEventsToBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[1] == null || !(values[1] is DateTime))
                return false;

            var currentDate = (DateTime)values[1];

            if (values[0] == null || !(values[0] is Dictionary<DateTime, ObservableCollection<CalendarEventCardVM>>))
                return false;

            var eventCardVMs = (Dictionary<DateTime, ObservableCollection<CalendarEventCardVM>>)values[0];
            if (eventCardVMs.ContainsKey(currentDate) && eventCardVMs[currentDate].Count > 0)
                return true;
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}