using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TienIchLich.ViewModels;
using System.Collections.Specialized;

namespace TienIchLich.MonthEventCalendarControl
{
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
        private CalendarVM oldCalendarVM; // Calendar view model before property change. Used for removing event handlers.

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
            this.Unloaded += MonthEventCalendar_Unloaded;
        }

        /// <summary>
        /// Remove all event handlers when control is unloaded.
        /// </summary>
        private void MonthEventCalendar_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.oldCalendarVM != null)
            {
                foreach (CalendarCategoryVM categoryVM in this.oldCalendarVM.CalendarCategoryVMs)
                    categoryVM.PropertyChanged -= CategoryVM_PropertyChanged;

                this.oldCalendarVM.CalendarCategoryVMs.CollectionChanged -= CalendarCategories_CollectionChanged;
            }
        }

        private static void OnCalendarVMPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (MonthEventCalendar)d;
            obj.SetOldCalendarVM();
            obj.AttachEventHandlersToCalendarCategoryVMs();
        }

        public void SetOldCalendarVM()
        {
            if (this.CalendarVM != null)
                this.oldCalendarVM = this.CalendarVM;
        }

        /// <summary>
        /// Allow calendar to be refreshed upon change of CalendarCategoryVM.IsDisplayed property
        /// by adding an event handler to its PropertyChanged event.
        /// </summary>
        private void AttachEventHandlersToCalendarCategoryVMs()
        {
            if (this.CalendarVM != null)
            {
                foreach (CalendarCategoryVM categoryVM in this.CalendarVM.CalendarCategoryVMs)
                    categoryVM.PropertyChanged += CategoryVM_PropertyChanged;

                this.CalendarVM.CalendarCategoryVMs.CollectionChanged += CalendarCategories_CollectionChanged;
            }
        }

        /// <summary>
        /// Add calendar refreshing event handler to newly added calendar category view model.
        /// </summary>
        private void CalendarCategories_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                CalendarCategoryVM newCategory = (CalendarCategoryVM)e.NewItems[0];
                newCategory.PropertyChanged += CategoryVM_PropertyChanged;
            }
        }

        /// <summary>
        /// Refresh calendar when a category is updated.
        /// </summary>
        private void CategoryVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DateTime? currentSelectedDate = this.SelectedDate;
            this.SelectedDate = new DateTime();
            this.SelectedDate = currentSelectedDate;
        }
    }
}
