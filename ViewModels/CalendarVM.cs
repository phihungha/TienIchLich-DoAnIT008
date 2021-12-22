using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
                eventVM.PropertyChanged += EventVM_PropertyChanged;
            }
        }

        private void CalendarEventVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newEvent = (CalendarEventVM)e.NewItems[0];
                AddCardVMsOfEventVM(newEvent);
                newEvent.PropertyChanged += EventVM_PropertyChanged;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var removedEvent = (CalendarEventVM)e.OldItems[0];
                RemoveCardVMsOfEventVM(removedEvent);
            }
        }

        private void EventVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "StartTime" || e.PropertyName == "EndTime")
            {
                var changedEvent = (CalendarEventVM)sender;
                RemoveCardVMsOfEventVM(changedEvent);
                AddCardVMsOfEventVM((CalendarEventVM)sender);
                RequestRefresh();
            }
        }

        /// <summary>
        /// Add all event card view models of provided calendar event view model.
        /// </summary>
        /// <param name="eventVM">Calendar event view model.</param>
        private void AddCardVMsOfEventVM(CalendarEventVM eventVM)
        {
            foreach (var entry in eventVM.EventCardVMs)
            {
                if (!EventCardVMs.ContainsKey(entry.Key))
                    EventCardVMs.Add(entry.Key, new ObservableCollection<CalendarEventCardVM>());
                EventCardVMs[entry.Key].Add(entry.Value);
            }
        }

        /// <summary>
        /// Remove all event card view models of provided calendar event view model.
        /// </summary>
        /// <param name="eventVM"></param>
        private void RemoveCardVMsOfEventVM(CalendarEventVM eventVM)
        {
            foreach (var entry in eventVM.EventCardVMs)
            {
                EventCardVMs[entry.Key].Remove(entry.Value);
                if (EventCardVMs[entry.Key].Count == 0)
                    EventCardVMs.Remove(entry.Key);
            }
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