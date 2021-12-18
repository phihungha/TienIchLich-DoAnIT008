using System;
using System.Collections.Specialized;
using System.ComponentModel;
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

        // The old calendar view model before property change. Used for removing unused event handlers.
        private CalendarVM oldCalendarVM;

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
            Unloaded += MonthEventCalendar_Unloaded;
        }

        protected override void OnGotMouseCapture(MouseEventArgs e)
        {
            UIElement originalElement = e.OriginalSource as UIElement;
            if (originalElement is CalendarDayButton || originalElement is CalendarItem)
                originalElement.ReleaseMouseCapture();
        }

        /// <summary>
        /// Remove unused event handlers when this control is unloaded.
        /// </summary>
        private void MonthEventCalendar_Unloaded(object sender, RoutedEventArgs e)
        {
            if (oldCalendarVM != null)
            {
                foreach (CalendarCategoryVM categoryVM in oldCalendarVM.CalendarCategoryVMs)
                    categoryVM.PropertyChanged -= VM_PropertyChanged;
                oldCalendarVM.CalendarCategoryVMs.CollectionChanged -= CalendarCategoryVMs_CollectionChanged;

                foreach (CalendarEventVM eventVM in oldCalendarVM.CalendarEventVMs)
                    eventVM.PropertyChanged -= VM_PropertyChanged;
                oldCalendarVM.CalendarEventVMs.CollectionChanged -= CalendarEventVMs_CollectionChanged;
            }
        }

        private static void OnCalendarVMPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (MonthEventCalendar)d;
            obj.SetOldCalendarVM();
            obj.AttachPropertyChangeEventHandlerToVMs();
        }

        public void SetOldCalendarVM()
        {
            if (CalendarVM != null)
                oldCalendarVM = CalendarVM;
        }

        /// <summary>
        /// Attach property change event handler to calendar category and events view models
        /// so the calendar is refreshed when they change.
        /// </summary>
        private void AttachPropertyChangeEventHandlerToVMs()
        {
            if (CalendarVM != null)
            {
                foreach (CalendarCategoryVM categoryVM in CalendarVM.CalendarCategoryVMs)
                    categoryVM.PropertyChanged += VM_PropertyChanged;
                CalendarVM.CalendarCategoryVMs.CollectionChanged += CalendarCategoryVMs_CollectionChanged;

                foreach (CalendarEventVM eventVM in CalendarVM.CalendarEventVMs)
                    eventVM.PropertyChanged += VM_PropertyChanged;
                CalendarVM.CalendarEventVMs.CollectionChanged += CalendarEventVMs_CollectionChanged;
            }
        }

        /// <summary>
        /// Add calendar refreshing event handler to a newly added calendar event view model.
        /// </summary>
        private void CalendarEventVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                CalendarEventVM newEvent = (CalendarEventVM)e.NewItems[0];
                newEvent.PropertyChanged += VM_PropertyChanged;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
                Refresh();
        }

        /// <summary>
        /// Add calendar refreshing event handler to a newly added calendar category view model.
        /// </summary>
        private void CalendarCategoryVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                CalendarCategoryVM newCategory = (CalendarCategoryVM)e.NewItems[0];
                newCategory.PropertyChanged += VM_PropertyChanged;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
                Refresh();
        }

        private void VM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Refresh();
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