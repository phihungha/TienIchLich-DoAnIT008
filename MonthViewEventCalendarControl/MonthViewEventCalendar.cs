using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace TienIchLich.MonthViewEventCalendarControl
{
    public class MonthViewEventCalendar : Calendar
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

        static MonthViewEventCalendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonthViewEventCalendar), 
                                                     new FrameworkPropertyMetadata(typeof(MonthViewEventCalendar)));
        }

        public MonthViewEventCalendar()
            : base()
        {
            SetValue(CalendarEventsProperty, new ObservableCollection<CalendarEventViewModel>());
        }

        //protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        //{
        //    base.OnMouseDoubleClick(e);

        //    FrameworkElement element = e.OriginalSource as FrameworkElement;

        //    if (element.DataContext is DateTime)
        //    {
        //        EventEditor eventEditor = new EventEditor
        //        (
        //            (CalendarEventViewModel calendarEvent) =>
        //            {
        //                Appointments.Add(calendarEvent);
        //                if (PropertyChanged != null)
        //                {
        //                    PropertyChanged(this, new PropertyChangedEventArgs("Appointments"));
        //                }
        //            }
        //        );
        //        appointmentWindow.Show();
        //    }
        //}
    }
}
