using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace TienIchLich.MonthEventCalendarControl
{
    public class MonthEventCalendar : Calendar
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static DependencyProperty CalendarEventsProperty =
            DependencyProperty.Register
            (
                "CalendarEvents",
                typeof(ObservableCollection<CalendarEventViewModel>),
                typeof(Calendar)
            );

        public ObservableCollection<CalendarEventViewModel> CalendarEvents
        {
            get { return (ObservableCollection<CalendarEventViewModel>)GetValue(CalendarEventsProperty); }
            set { SetValue(CalendarEventsProperty, value); }
        }

        static MonthEventCalendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonthEventCalendar), 
                                                     new FrameworkPropertyMetadata(typeof(MonthEventCalendar)));
        }

        public MonthEventCalendar()
            : base()
        {
            SetValue(CalendarEventsProperty, new ObservableCollection<CalendarEventViewModel>());
        }
    }
}
