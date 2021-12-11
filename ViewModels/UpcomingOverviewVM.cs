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
        private CollectionViewSource eventCollectionViewSource;

        /// <summary>
        /// Collection view for the upcoming event DataGrid.
        /// </summary>
        public ICollectionView EventCollectionView => eventCollectionViewSource.View;

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
                EventCollectionView.Refresh();
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

        public UpcomingOverviewVM(ObservableCollection<CalendarEventVM> eventVMs)
        {
            eventCollectionViewSource = new CollectionViewSource()
            {
                Source = eventVMs,
                IsLiveFilteringRequested = true,
                IsLiveSortingRequested = true,
                IsLiveGroupingRequested = true
            };
            AttachEventHandlersToCalendarEventVMs(eventVMs);
            eventCollectionViewSource.Filter += EventCollectionViewSource_Filter;
            eventCollectionViewSource.GroupDescriptions.Add(new PropertyGroupDescription("StartTime.Date"));
            eventCollectionViewSource.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Ascending));
            eventCollectionViewSource.LiveFilteringProperties.Add("CategoryVM.IsDisplayed");
            eventCollectionViewSource.LiveSortingProperties.Add("StartTime");
            eventCollectionViewSource.LiveGroupingProperties.Add("StartTime");

            SelectedStartTimeFilterOption = StartTimeFilterOptions[0];
        }

        /// <summary>
        /// Get upcoming events that satisfy the filters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventCollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            var eventVM = (CalendarEventVM)e.Item;
            bool startTimeAccepted = SelectedStartTimeFilterOption.Id == UpcomingOverviewStartTimeFilterOptionId.All ||
                                     (eventVM.StartTime < (DateTime.Now + StartTimeFilterValue)
                                      && eventVM.StartTime > DateTime.Now);
            e.Accepted = eventVM.CategoryVM.IsDisplayed && startTimeAccepted;
        }

        // All of the code below is to add the sort description to the collection view again because it changes
        // when we enter the event editor to add/edit event.

        private void AttachEventHandlersToCalendarEventVMs(ObservableCollection<CalendarEventVM> eventVMs)
        {
            eventVMs.CollectionChanged += EventVMs_CollectionChanged;
            foreach (CalendarEventVM eventVM in eventVMs)
                eventVM.PropertyChanged += DataVMChanged;
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

            EventCollectionView.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Ascending));
        }

        private void DataVMChanged(object sender, PropertyChangedEventArgs e)
        {
            eventCollectionViewSource.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Ascending));
        }
    }
}