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

        private static void OnCalendarVMPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (MonthEventCalendar)d;
            obj.AttachEventHandlersToCalendarCategoryVMs();
        }

        /// <summary>
        /// Allow calendar to be refreshed upon change in CalendarCategoryVM.IsDisplayed property
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
            if (e.NewItems != null)
            {
                CalendarCategoryVM newCategory = (CalendarCategoryVM)(e.NewItems[0]);
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
