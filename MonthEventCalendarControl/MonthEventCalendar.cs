using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TienIchLich.ViewModels;

namespace TienIchLich.MonthEventCalendarControl
{
    public class MonthEventCalendar : Calendar
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static DependencyProperty CalendarEventsProperty =
            DependencyProperty.Register
            (
                "CalendarEvents",
                typeof(ObservableCollection<CalendarEventVM>),
                typeof(Calendar)
            );

        public ObservableCollection<CalendarEventVM> CalendarEvents
        {
            get { return (ObservableCollection<CalendarEventVM>)GetValue(CalendarEventsProperty); }
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
            SetValue(CalendarEventsProperty, new ObservableCollection<CalendarEventVM>());
        }
     }
}
