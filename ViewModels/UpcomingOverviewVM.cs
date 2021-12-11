using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model for the upcoming event overview.
    /// </summary>
    public class UpcomingOverviewVM : ViewModelBase
    {
        /// <summary>
        /// Identifiers for start time filter options.
        /// </summary>
        public enum StartTimeFilterOptionId
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
        public struct StartTimeFilterOption
        {
            /// <summary>
            /// Identifier of this option.
            /// </summary>
            public StartTimeFilterOptionId Id { get; set; }

            /// <summary>
            /// Time filter value of this option.
            /// </summary>
            public TimeSpan Time { get; set; }
        }

        private CollectionViewSource eventCollectionViewSource;

        /// <summary>
        /// Collection view for the upcoming event DataGrid.
        /// </summary>
        public ICollectionView EventCollectionView => eventCollectionViewSource.View;

        private static StartTimeFilterOption[] startTimeFilterOptions =
        {
            new StartTimeFilterOption() { Id = StartTimeFilterOptionId.Week1, Time = new TimeSpan(7, 0, 0, 0) },
            new StartTimeFilterOption() { Id = StartTimeFilterOptionId.Week2, Time = new TimeSpan(14, 0, 0, 0) },
            new StartTimeFilterOption() { Id = StartTimeFilterOptionId.Month1, Time = new TimeSpan(31, 0, 0, 0) },
            new StartTimeFilterOption() { Id = StartTimeFilterOptionId.Month6, Time = new TimeSpan(186, 0, 0, 0) },
            new StartTimeFilterOption() { Id = StartTimeFilterOptionId.Year1, Time = new TimeSpan(366, 0, 0, 0) },
            new StartTimeFilterOption() { Id = StartTimeFilterOptionId.All },
            new StartTimeFilterOption() { Id = StartTimeFilterOptionId.Custom, Time = new TimeSpan(1, 0, 0, 0) }
        };

        /// <summary>
        /// Start time filter options.
        /// </summary>
        public StartTimeFilterOption[] StartTimeFilterOptions => startTimeFilterOptions;

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

        private StartTimeFilterOption selectedStartTimeFilterOption;

        /// <summary>
        /// Selected tart time filter option.
        /// </summary>
        public StartTimeFilterOption SelectedStartTimeFilterOption
        {
            get
            {
                return selectedStartTimeFilterOption;
            }
            set
            {
                selectedStartTimeFilterOption = value;
                if (value.Id == StartTimeFilterOptionId.Custom)
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
            bool startTimeAccepted = SelectedStartTimeFilterOption.Id == StartTimeFilterOptionId.All ||
                                     (eventVM.StartTime < (DateTime.Now + StartTimeFilterValue)
                                      && eventVM.StartTime > DateTime.Now);
            e.Accepted = eventVM.CategoryVM.IsDisplayed && startTimeAccepted;
        }

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