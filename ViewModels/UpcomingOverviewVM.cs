using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// Identifiers for start time filter options.
    /// </summary>
    public enum UpcomingOverviewStartTimeFilterOptionId
    {
        Week1,
        Week2,
        Month1,
        Month6,
        Year1,
        All,
        Custom
    }

    /// <summary>
    /// Info of a start time filter option.
    /// </summary>
    public struct UpcomingOverviewStartTimeFilterOption
    {
        /// <summary>
        /// Identifier of this option.
        /// </summary>
        public UpcomingOverviewStartTimeFilterOptionId Id { get; set; }

        /// <summary>
        /// Time filter value of this option.
        /// </summary>
        public TimeSpan Time { get; set; }
    }

    /// <summary>
    /// View model for the upcoming event overview.
    /// </summary>
    public class UpcomingOverviewVM : ViewModelBase
    {
        private ObservableCollection<CalendarEventVM> eventVMs;
        private ObservableCollection<CalendarCategoryVM> categoryVMs;

        /// <summary>
        /// Collection view for the upcoming event DataGrid.
        /// </summary>
        public ObservableCollection<CalendarEventVM> UpcomingEventVMs { get; private set; }


        private static UpcomingOverviewStartTimeFilterOption[] startTimeFilterOptions =
        {
            new UpcomingOverviewStartTimeFilterOption() { Id = UpcomingOverviewStartTimeFilterOptionId.Week1, Time = new TimeSpan(7, 0, 0, 0) },
            new UpcomingOverviewStartTimeFilterOption() { Id = UpcomingOverviewStartTimeFilterOptionId.Week2, Time = new TimeSpan(14, 0, 0, 0) },
            new UpcomingOverviewStartTimeFilterOption() { Id = UpcomingOverviewStartTimeFilterOptionId.Month1, Time = new TimeSpan(31, 0, 0, 0) },
            new UpcomingOverviewStartTimeFilterOption() { Id = UpcomingOverviewStartTimeFilterOptionId.Month6, Time = new TimeSpan(186, 0, 0, 0) },
            new UpcomingOverviewStartTimeFilterOption() { Id = UpcomingOverviewStartTimeFilterOptionId.Year1, Time = new TimeSpan(366, 0, 0, 0) },
            new UpcomingOverviewStartTimeFilterOption() { Id = UpcomingOverviewStartTimeFilterOptionId.All },
            new UpcomingOverviewStartTimeFilterOption() { Id = UpcomingOverviewStartTimeFilterOptionId.Custom, Time = new TimeSpan(1, 0, 0, 0) }
        };

        /// <summary>
        /// Start time filter options.
        /// </summary>
        public UpcomingOverviewStartTimeFilterOption[] StartTimeFilterOptions => startTimeFilterOptions;

        private bool useCustomStartTimeFilter;

        /// <summary>
        /// True if custom start time filter is used.
        /// </summary>
        public bool UseCustomStartTimeFilter
        {
            get
            {
                return useCustomStartTimeFilter;
            }
            set
            {
                useCustomStartTimeFilter = value;
                NotifyPropertyChanged();
            }
        }

        private TimeSpan startTimeFilterValue;

        /// <summary>
        /// Current start time filter value.
        /// </summary>
        public TimeSpan StartTimeFilterValue
        {
            get
            {
                return startTimeFilterValue;
            }
            set
            {
                startTimeFilterValue = value;
                GetUpcomingEventVMs();
                NotifyPropertyChanged();
            }
        }

        private UpcomingOverviewStartTimeFilterOption selectedStartTimeFilterOption;

        /// <summary>
        /// Selected tart time filter option.
        /// </summary>
        public UpcomingOverviewStartTimeFilterOption SelectedStartTimeFilterOption
        {
            get
            {
                return selectedStartTimeFilterOption;
            }
            set
            {
                selectedStartTimeFilterOption = value;
                if (value.Id == UpcomingOverviewStartTimeFilterOptionId.Custom)
                    UseCustomStartTimeFilter = true;
                else
                    UseCustomStartTimeFilter = false;
                StartTimeFilterValue = value.Time;
                NotifyPropertyChanged();
            }
        }

        public UpcomingOverviewVM(ObservableCollection<CalendarEventVM> eventVMs, ObservableCollection<CalendarCategoryVM> categoryVMs)
        {
            this.eventVMs = eventVMs;
            this.categoryVMs = categoryVMs;
            UpcomingEventVMs = new();
            GetUpcomingEventVMs();
            AttachEventHandlersToCalendarDataVMs();
            SelectedStartTimeFilterOption = StartTimeFilterOptions[0];
        }

        /// <summary>
        /// Get all upcoming events that satisfy filters.
        /// </summary>
        private void GetUpcomingEventVMs()
        {
            UpcomingEventVMs.Clear();
            foreach (CalendarEventVM eventVM in eventVMs)
            {
                bool startTimeAccepted = SelectedStartTimeFilterOption.Id == UpcomingOverviewStartTimeFilterOptionId.All ||
                                         (eventVM.StartTime < (DateTime.Now + StartTimeFilterValue)
                                          && eventVM.StartTime > DateTime.Now);
                if (startTimeAccepted && eventVM.CategoryVM.IsDisplayed)
                    UpcomingEventVMs.Add(eventVM);
            }
        }

        /// <summary>
        /// Attach filter refresh event handlers for calendar category and event property change.
        /// </summary>
        private void AttachEventHandlersToCalendarDataVMs()
        {
            eventVMs.CollectionChanged += EventVMs_CollectionChanged;
            foreach (CalendarEventVM eventVM in eventVMs)
                eventVM.PropertyChanged += DataVMChanged;

            categoryVMs.CollectionChanged += CategoryVMs_CollectionChanged;
            foreach (CalendarCategoryVM categoryVM in categoryVMs)
                categoryVM.PropertyChanged += DataVMChanged;
        }

        private void CategoryVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                CalendarCategoryVM categoryVM = (CalendarCategoryVM)e.NewItems[0];
                categoryVM.PropertyChanged += DataVMChanged;
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                CalendarCategoryVM categoryVM = (CalendarCategoryVM)e.OldItems[0];
                categoryVM.PropertyChanged -= DataVMChanged;
            }
        }

        private void EventVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                CalendarEventVM eventVM = (CalendarEventVM)e.NewItems[0];
                eventVM.PropertyChanged += DataVMChanged;
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                CalendarEventVM eventVM = (CalendarEventVM)e.OldItems[0];
                eventVM.PropertyChanged -= DataVMChanged;
            }
            GetUpcomingEventVMs();
        }

        private void DataVMChanged(object sender, PropertyChangedEventArgs e)
        {
            GetUpcomingEventVMs();
        }
    }
}