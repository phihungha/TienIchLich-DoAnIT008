using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
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
                typeof(Calendar),
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

        /// <summary>
        /// Remove unused event handlers when this control is unloaded.
        /// </summary>
        private void MonthEventCalendar_Unloaded(object sender, RoutedEventArgs e)
        {
            if (oldCalendarVM != null)
            {
                foreach (CalendarCategoryVM categoryVM in oldCalendarVM.CalendarCategoryVMs)
                    categoryVM.PropertyChanged -= CategoryVM_PropertyChanged;

                oldCalendarVM.CalendarCategoryVMs.CollectionChanged -= CalendarCategoryVMs_CollectionChanged;
            }
        }

        private static void OnCalendarVMPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (MonthEventCalendar)d;
            obj.SetOldCalendarVM();
            obj.AttachPropertyChangeEventHandlerToCalendarCategoryVMs();
        }

        public void SetOldCalendarVM()
        {
            if (CalendarVM != null)
                oldCalendarVM = CalendarVM;
        }

        /// <summary>
        /// Attach an event handler to calendar category view models' property change event
        /// so the calendar is refreshed when they change.
        /// </summary>
        private void AttachPropertyChangeEventHandlerToCalendarCategoryVMs()
        {
            if (CalendarVM != null)
            {
                foreach (CalendarCategoryVM categoryVM in CalendarVM.CalendarCategoryVMs)
                    categoryVM.PropertyChanged += CategoryVM_PropertyChanged;

                CalendarVM.CalendarCategoryVMs.CollectionChanged += CalendarCategoryVMs_CollectionChanged;
            }
        }

        /// <summary>
        /// Add calendar refreshing event handler to a newly added calendar category view model.
        /// </summary>
        private void CalendarCategoryVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                CalendarCategoryVM newCategory = (CalendarCategoryVM)e.NewItems[0];
                newCategory.PropertyChanged += CategoryVM_PropertyChanged;
            }
        }

        /// <summary>
        /// Refresh calendar when a calendar category view model changes.
        /// </summary>
        private void CategoryVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DateTime? currentSelectedDate = SelectedDate;
            SelectedDate = new DateTime();
            SelectedDate = currentSelectedDate;
        }
    }
}