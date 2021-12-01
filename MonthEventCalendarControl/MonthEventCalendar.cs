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

        public static DependencyProperty CalendarVMProperty =
            DependencyProperty.Register
            (
                "CalendarVM",
                typeof(CalendarVM),
                typeof(Calendar)
            );

        public CalendarVM CalendarVM
        {
            get { return (CalendarVM)GetValue(CalendarVMProperty); }
            set { SetValue(CalendarVMProperty, value); }
        }

        static MonthEventCalendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonthEventCalendar), 
                                                     new FrameworkPropertyMetadata(typeof(MonthEventCalendar)));
        }

        public MonthEventCalendar()
            : base()
        {
            SetValue(CalendarVMProperty, new CalendarVM());
        }
     }
}
