using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

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
        public ObservableCollection<CalendarEventCardVM> UpcomingEventCardVMs { get; private set; }

        private static UpcomingOverviewStartTimeFilterOption[] startTimeFilterOptions =
        {
            new UpcomingOverviewStartTimeFilterOption() 
            { 
                Id = UpcomingOverviewStartTimeFilterOptionId.Week1, 
                Time = new TimeSpan(7, 0, 0, 0) 
            },
            new UpcomingOverviewStartTimeFilterOption() 
            { 
                Id = UpcomingOverviewStartTimeFilterOptionId.Week2, 
                Time = new TimeSpan(14, 0, 0, 0) },
            new UpcomingOverviewStartTimeFilterOption() 
            { 
                Id = UpcomingOverviewStartTimeFilterOptionId.Month1, 
                Time = new TimeSpan(31, 0, 0, 0) },
            new UpcomingOverviewStartTimeFilterOption() 
            { 
                Id = UpcomingOverviewStartTimeFilterOptionId.Month6, 
                Time = new TimeSpan(186, 0, 0, 0) },
            new UpcomingOverviewStartTimeFilterOption() 
            { 
                Id = UpcomingOverviewStartTimeFilterOptionId.Year1, 
                Time = new TimeSpan(366, 0, 0, 0) },
            new UpcomingOverviewStartTimeFilterOption() 
            { 
                Id = UpcomingOverviewStartTimeFilterOptionId.All 
            },
            new UpcomingOverviewStartTimeFilterOption() 
            { 
                Id = UpcomingOverviewStartTimeFilterOptionId.Custom, 
                Time = new TimeSpan(1, 0, 0, 0) 
            }
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
            UpcomingEventCardVMs = new ObservableCollection<CalendarEventCardVM>();
            GetUpcomingEventVMs();
            AttachEventHandlersToCalendarDataVMs();
            SelectedStartTimeFilterOption = StartTimeFilterOptions[0];
        }

        /// <summary>
        /// Get all upcoming events that satisfy filters.
        /// </summary>
        private void GetUpcomingEventVMs()
        {
            UpcomingEventCardVMs.Clear();
            foreach (CalendarEventVM eventVM in eventVMs)
            {
                if (eventVM.CategoryVM.IsDisplayed)
                {
                    foreach (var entry in eventVM.EventCardVMs)
                    {
                        CalendarEventCardVM cardVM = entry.Value;
                        if (SelectedStartTimeFilterOption.Id != UpcomingOverviewStartTimeFilterOptionId.All)
                        {
                            if (cardVM.IsFirstDay
                                && cardVM.EventVM.StartTime <= (DateTime.Now + StartTimeFilterValue)
                                && cardVM.EventVM.StartTime > DateTime.Now)
                                 UpcomingEventCardVMs.Add(cardVM);
                            else if (cardVM.DateOnCalendar <= (DateTime.Now + StartTimeFilterValue))
                                UpcomingEventCardVMs.Add(cardVM);
                        }
                        else
                            UpcomingEventCardVMs.Add(cardVM);
                    }
                }
            }
        }

        /// <summary>
        /// Attach filter refresh event handlers for calendar category and event card change.
        /// </summary>
        private void AttachEventHandlersToCalendarDataVMs()
        {
            eventVMs.CollectionChanged += EventVMs_CollectionChanged;
            foreach (CalendarEventVM eventVM in eventVMs)
                eventVM.RequestAddEventCardVM += EventVM_RequestAddEventCardVM;

            categoryVMs.CollectionChanged += CategoryVMs_CollectionChanged;
            foreach (CalendarCategoryVM categoryVM in categoryVMs)
                categoryVM.PropertyChanged += CategoryVM_PropertyChanged;
        }

        private void CategoryVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                CalendarCategoryVM categoryVM = (CalendarCategoryVM)e.NewItems[0];
                categoryVM.PropertyChanged += CategoryVM_PropertyChanged;
            }
        }

        private void EventVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                CalendarEventVM eventVM = (CalendarEventVM)e.NewItems[0];
                eventVM.RequestAddEventCardVM += EventVM_RequestAddEventCardVM;
            }
            GetUpcomingEventVMs();
        }

        private void EventVM_RequestAddEventCardVM(CalendarEventVM sender)
        {
            GetUpcomingEventVMs();
        }

        private void CategoryVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            GetUpcomingEventVMs();
        }
    }
}