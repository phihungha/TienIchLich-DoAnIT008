using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

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

        /// <summary>
        /// A collection of upcoming event cards.
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
                LoadCardVMsOfAllEventVMs();
                NotifyPropertyChanged();
            }
        }

        private UpcomingOverviewStartTimeFilterOption selectedStartTimeFilterOption;

        /// <summary>
        /// Selected start time filter option.
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
            this.eventVMs = eventVMs;
            eventVMs.CollectionChanged += EventVMs_CollectionChanged;
            UpcomingEventCardVMs = new ObservableCollection<CalendarEventCardVM>();
            LoadCardVMsOfAllEventVMs();
            SelectedStartTimeFilterOption = StartTimeFilterOptions[0];
        }

        /// <summary>
        /// Load all upcoming event cards of all events.
        /// </summary>
        private void LoadCardVMsOfAllEventVMs()
        {
            UpcomingEventCardVMs.Clear();
            foreach (CalendarEventVM eventVM in eventVMs)
            {
                LoadUpcomingEventCardVMs(eventVM.EventCardVMs);
                eventVM.EventCardVMsAdded += EventVM_AddEventCardVMsAdded;
                eventVM.EventCardVMsRemoved += EventVM_EventCardVMsRemoved;
            }
        }

        /// <summary>
        /// Load all event cards that satisfy upcoming event filter.
        /// </summary>
        /// <param name="cardVMs"></param>
        private void LoadUpcomingEventCardVMs(Dictionary<DateTime,CalendarEventCardVM> cardVMs)
        {
            foreach (CalendarEventCardVM cardVM in cardVMs.Values)
            {
                if (SelectedStartTimeFilterOption.Id != UpcomingOverviewStartTimeFilterOptionId.All)
                {
                    if (cardVM.IsFirstDay
                        && cardVM.EventVM.StartTime <= (DateTime.Now + StartTimeFilterValue)
                        && cardVM.EventVM.StartTime >= DateTime.Now)
                        UpcomingEventCardVMs.Add(cardVM);
                    else if (cardVM.DateOnCalendar <= (DateTime.Now + StartTimeFilterValue)
                             && cardVM.DateOnCalendar >= DateTime.Now.Date)
                        UpcomingEventCardVMs.Add(cardVM);
                }
                else
                    UpcomingEventCardVMs.Add(cardVM);
            }
        }

        /// <summary>
        /// Update upcoming event card collection when an event is added/removed.
        /// </summary>
        private void EventVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                CalendarEventVM eventVM = (CalendarEventVM)e.NewItems[0];
                LoadUpcomingEventCardVMs(eventVM.EventCardVMs);
                eventVM.EventCardVMsAdded += EventVM_AddEventCardVMsAdded;
                eventVM.EventCardVMsRemoved += EventVM_EventCardVMsRemoved;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var removedEvent = (CalendarEventVM)e.OldItems[0];
                EventVM_EventCardVMsRemoved(removedEvent);
            }
        }

        /// <summary>
        /// Remove outdated event cards after an event changes.
        /// </summary>
        private void EventVM_EventCardVMsRemoved(CalendarEventVM sender)
        {
            foreach (CalendarEventCardVM cardVM in sender.EventCardVMs.Values)
                UpcomingEventCardVMs.Remove(cardVM);
        }

        /// <summary>
        /// Add new event cards after an event changes.
        /// </summary>
        private void EventVM_AddEventCardVMsAdded(CalendarEventVM sender)
        {
            LoadUpcomingEventCardVMs(sender.EventCardVMs);
        }
    }
}