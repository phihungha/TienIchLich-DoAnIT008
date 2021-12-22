using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model for a calendar view.
    /// </summary>
    public class CalendarVM : ViewModelBase
    {
        private MainWorkspaceVM mainWorkspaceVM;

        public delegate void RequestRefreshHandler();

        /// <summary>
        /// Request a calendar control refresh.
        /// </summary>
        public event RequestRefreshHandler RequestRefresh;

        /// <summary>
        /// Calendar event card view models for calendar controls to display.
        /// </summary>
        public Dictionary<DateTime, ObservableCollection<CalendarEventCardVM>> EventCardVMs { get; private set; }

        /// <summary>
        /// Calendar event view models to display.
        /// </summary>
        public ObservableCollection<CalendarEventVM> CalendarEventVMs { get; private set; }

        /// <summary>
        /// Calendar category view models for the calendar control to monitor
        /// and update itself when they change.
        /// </summary>
        public ObservableCollection<CalendarCategoryVM> CalendarCategoryVMs { get; private set; }

        private DateTime? monthCalendarSelectedDate = DateTime.Now;

        /// <summary>
        /// Selected date on the calendar control.
        /// </summary>
        public DateTime? MonthCalendarSelectedDate
        {
            get
            {
                return monthCalendarSelectedDate;
            }
            set
            {
                monthCalendarSelectedDate = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime monthCalendarDisplayDate = DateTime.Now;

        /// <summary>
        /// Display date on the calendar control.
        /// </summary>
        public DateTime MonthCalendarDisplayDate
        {
            get
            {
                return monthCalendarDisplayDate;
            }
            set
            {
                monthCalendarDisplayDate = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime? yearCalendarSelectedDate = DateTime.Now;

        public DateTime? YearCalendarSelectedDate
        {
            get
            {
                return yearCalendarSelectedDate;
            }
            set
            {
                yearCalendarSelectedDate = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Command to add new calendar event.
        /// </summary>
        public ICommand AddEventCommand { get; private set; }

        /// <summary>
        /// Command to jump to today's date and select it.
        /// </summary>
        public ICommand TodayCommand { get; private set; }

        /// <summary>
        /// Command to jump to a different month on month calendar from year calendar.
        /// </summary>
        public ICommand JumpToAnotherMonthCommand { get; private set; }

        public CalendarVM(ObservableCollection<CalendarEventVM> eventVMs, ObservableCollection<CalendarCategoryVM> categoryVMs, NavigationVM navigationVM, MainWorkspaceVM mainWorkspaceVM)
        {
            this.mainWorkspaceVM = mainWorkspaceVM;
            CalendarEventVMs = eventVMs;
            CalendarCategoryVMs = categoryVMs;
            CalendarEventVMs.CollectionChanged += CalendarEventVMs_CollectionChanged;
            EventCardVMs = new();

            AddEventCommand = new RelayCommand(
                i => navigationVM.NavigateToEventEditorViewToAdd(MonthCalendarSelectedDate));
            TodayCommand = new RelayCommand(i => JumpToToday());
            JumpToAnotherMonthCommand = new RelayCommand(i => JumpToAnotherMonth());

            LoadCardVMsOfAllEventVMs();
        }

        /// <summary>
        /// Load event card view models of all calendar event view models.
        /// </summary>
        private void LoadCardVMsOfAllEventVMs()
        {
            foreach (CalendarEventVM eventVM in CalendarEventVMs)
            {
                AddCardVMsOfEventVM(eventVM);
                eventVM.RequestAddEventCardVM += AddCardVMsOfEventVM;
                eventVM.RequestRemoveEventCardVM += RemoveCardVMsOfEventVM;
            }
        }

        /// <summary>
        /// Update event card dictionary when a new event view model is added/removed.
        /// </summary>
        private void CalendarEventVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newEvent = (CalendarEventVM)e.NewItems[0];
                AddCardVMsOfEventVM(newEvent);
                newEvent.RequestAddEventCardVM += AddCardVMsOfEventVM;
                newEvent.RequestRemoveEventCardVM += RemoveCardVMsOfEventVM;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var removedEvent = (CalendarEventVM)e.OldItems[0];
                RemoveCardVMsOfEventVM(removedEvent);
            }
        }

        /// <summary>
        /// Add all event card view models of provided calendar event view model.
        /// </summary>
        /// <param name="eventVM">Calendar event view model.</param>
        private void AddCardVMsOfEventVM(CalendarEventVM eventVM)
        {
            bool needRefresh = false;
            foreach (var entry in eventVM.EventCardVMs)
            {
                if (!EventCardVMs.ContainsKey(entry.Key))
                {
                    EventCardVMs.Add(entry.Key, new ObservableCollection<CalendarEventCardVM>());
                    needRefresh = true;
                }
                EventCardVMs[entry.Key].Add(entry.Value);
            }

            if (needRefresh)
                RequestRefresh?.Invoke();
        }

        /// <summary>
        /// Remove all event card view models of provided calendar event view model.
        /// </summary>
        /// <param name="eventVM"></param>
        private void RemoveCardVMsOfEventVM(CalendarEventVM eventVM)
        {
            bool needRefresh = false;
            foreach (var entry in eventVM.EventCardVMs)
            {
                EventCardVMs[entry.Key].Remove(entry.Value);
                if (EventCardVMs[entry.Key].Count == 0)
                {
                    EventCardVMs.Remove(entry.Key);
                    needRefresh = true;
                }
            }

            if (needRefresh)
                RequestRefresh?.Invoke();
        }

        /// <summary>
        /// Jump to current date and select it.
        /// </summary>
        private void JumpToToday()
        {
            MonthCalendarDisplayDate = DateTime.Now;
            MonthCalendarSelectedDate = DateTime.Now;
        }

        /// <summary>
        /// Jump to another month on month calendar.
        /// </summary>
        private void JumpToAnotherMonth()
        {
            if (YearCalendarSelectedDate != null)
            {
                MonthCalendarDisplayDate = (DateTime)YearCalendarSelectedDate;
                MonthCalendarSelectedDate = YearCalendarSelectedDate;
                mainWorkspaceVM.SelectedTabIndex = 1;
                Thread.Sleep(200);
            }
        }
    }
}