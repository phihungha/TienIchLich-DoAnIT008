using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using TienIchLich.ViewModels;

namespace TienIchLich.MonthEventCalendarControl
{
    /// <summary>
    /// A month calendar with event support.
    /// </summary>
    public class MonthEventCalendar : Calendar
    {
        public static DependencyProperty CalendarVMProperty =
            DependencyProperty.Register
            (
                "CalendarVM",
                typeof(CalendarVM),
                typeof(MonthEventCalendar),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    new PropertyChangedCallback(OnCalendarVMPropertyChanged))
            );

        /// <summary>
        /// View model for this calendar.
        /// </summary>
        public CalendarVM CalendarVM
        {
            get => (CalendarVM)GetValue(CalendarVMProperty);

            set => SetValue(CalendarVMProperty, value);
        }

        static MonthEventCalendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonthEventCalendar),
                                                     new FrameworkPropertyMetadata(typeof(MonthEventCalendar)));
        }

        public MonthEventCalendar()
            : base()
        {
        }

        protected override void OnGotMouseCapture(MouseEventArgs e)
        {
            UIElement originalElement = e.OriginalSource as UIElement;
            if (originalElement is CalendarDayButton || originalElement is CalendarItem)
                originalElement.ReleaseMouseCapture();
        }

        private static void OnCalendarVMPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (MonthEventCalendar)d;
            if (obj.CalendarVM != null)
                obj.AttachEventHandlerToCalendarVM();
        }

        private void AttachEventHandlerToCalendarVM()
        {
            CalendarVM.RequestRefresh += Refresh;
        }

        /// <summary>
        /// Refresh calendar when a calendar category view model changes.
        /// </summary>
        private void Refresh()
        {
            DateTime? currentSelectedDate = SelectedDate;
            SelectedDate = new DateTime();
            SelectedDate = currentSelectedDate;
        }
    }
}