using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using System.Windows.Input;
using TienIchLich.Services;
using TienIchLich.ViewModels.Converters;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model for calendar event display card on calendar views.
    /// </summary>
    public class CalendarEventCardVM : ViewModelBase
    {
        private string displaySubject;

        /// <summary>
        /// Subject name to display.
        /// </summary>
        public string DisplaySubject
        {
            get
            {
                return displaySubject;
            }
            set
            {
                displaySubject = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Day number since start time.
        /// </summary>
        public DateTime DateOnCalendar { get; set; }

        /// <summary>
        /// Number of days this event happens.
        /// </summary>
        public int TotalDayNum { get; set; }

        private CalendarEventVM eventVM;

        /// <summary>
        /// View model of this card's event.
        /// </summary>
        public CalendarEventVM EventVM
        {
            get
            {
                return eventVM;
            }
            set
            {
                eventVM = value;
                UpdateDisplaySubject();
                eventVM.PropertyChanged += EventVM_PropertyChanged;
            }
        }

        /// <summary>
        /// This event card is for the first day of the event.
        /// </summary>
        public bool IsFirstDay { get; set; }

        /// <summary>
        /// Update display subject name when event's subject changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Subject")
                UpdateDisplaySubject();
        }

        /// <summary>
        /// Set day components of display subject name.
        /// </summary>
        public void UpdateDisplaySubject()
        {
            if (TotalDayNum == 1)
                DisplaySubject = $"{EventVM.Subject}";
            else
            {
                int dayCount = (DateOnCalendar - EventVM.StartTime.Date).Days + 1;
                DisplaySubject = $"{EventVM.Subject} ({dayCount}/{TotalDayNum})";
            }
        }
    }

    /// <summary>
    /// View model of a calendar event.
    /// </summary>
    public class CalendarEventVM : ViewModelBase, IDisposable
    {
        private NavigationVM navigationVM;
        private CalendarEventVMManager eventVMManager;
        private DialogService dialogService;

        /// <summary>
        /// View models of event cards belong to this event.
        /// </summary>
        public Dictionary<DateTime, CalendarEventCardVM> EventCardVMs { get; private set; }

        public delegate void RequestRemoveEventCardVMHandler(CalendarEventVM sender);

        /// <summary>
        /// Request calendar view model to remove outdated event card view models.
        /// </summary>
        public event RequestRemoveEventCardVMHandler EventCardVMsRemoved;

        public delegate void RequestAddEventCardVMHandler(CalendarEventVM sender);

        /// <summary>
        /// Request calendar view model to add new event card view models.
        /// </summary>
        public event RequestAddEventCardVMHandler EventCardVMsAdded;

        private SmartTimer statusUpdateTimer = SmartTimerService.GetTimer();

        private CalendarEventStatusVM statusVM;

        /// <summary>
        /// Current status of this event.
        /// </summary>
        public CalendarEventStatusVM StatusVM
        {
            get
            {
                return statusVM;
            }
            private set
            {
                statusVM = value;
                NotifyPropertyChanged();
            }
        }

        private int id = 0;

        /// <summary>
        /// Id of this event in database for searching.
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                NotifyPropertyChanged();
            }
        }

        private string subject = "(Tên rỗng)";

        /// <summary>
        /// Subject of this event.
        /// </summary>
        public string Subject
        {
            get
            {
                return subject;
            }
            set
            {
                subject = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime startTime = DateTime.Now.Date.AddDays(1);

        /// <summary>
        /// This event's start time.
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    if (value <= EndTime)
                        SaveTimeChanges();
                    SetReminderTimer();
                    NotifyPropertyChanged();
                }
            }
        }

        private DateTime endTime = DateTime.Now.Date.AddDays(2);

        /// <summary>
        /// This event's end time.
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    if (value >= StartTime)
                        SaveTimeChanges();
                    NotifyPropertyChanged();
                }
            }
        }

        private bool allDay = true;

        /// <summary>
        /// True if this event happens in an entire day.
        /// </summary>
        public bool AllDay
        {
            get
            {
                return allDay;
            }
            set
            {
                allDay = value;
                NotifyPropertyChanged();
            }
        }

        private SmartTimer reminderTimer = SmartTimerService.GetTimer();

        private TimeSpan reminderTime = new TimeSpan(0, 30, 0);

        /// <summary>
        /// Time to remind the user before this event starts.
        /// </summary>
        public TimeSpan ReminderTime
        {
            get
            {
                return reminderTime;
            }
            set
            {
                if (reminderTime != value)
                {
                    reminderTime = value;
                    SetReminderTimer();
                    RemainingTime = reminderTime;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Remaining time until event happens.
        /// </summary>
        public TimeSpan RemainingTime { get; set; }

        private string description = "";

        /// <summary>
        /// Description of this event.
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                NotifyPropertyChanged();
            }
        }

        private CalendarCategoryVM categoryVM;

        /// <summary>
        /// View model for the calendar category of this event.
        /// </summary>
        public CalendarCategoryVM CategoryVM
        {
            get
            {
                return categoryVM;
            }
            set
            {
                if (categoryVM != null)
                    categoryVM.UnregisterEvent();
                categoryVM = value;
                categoryVM.RegisterEvent();
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Command to open event editor in edit mode for this event.
        /// </summary>
        public ICommand EditCommand { get; private set; }

        /// <summary>
        /// Command to delete this event.
        /// </summary>
        public ICommand DeleteCommand { get; private set; }

        public CalendarEventVM(CalendarEventVMManager eventVMManager, NavigationVM navigationVM, DialogService dialogService, DateTime? startTime = null)
        {
            this.navigationVM = navigationVM;
            this.dialogService = dialogService;
            this.eventVMManager = eventVMManager;
            EventCardVMs = new();

            if (startTime != null)
            {
                StartTime = (DateTime)startTime;
                EndTime = StartTime.AddDays(1);
            }
            else
            {
                SetReminderTimer();
                SetStatus();
            }

            statusUpdateTimer.Elapsed += StatusUpdateTimer_Elapsed;
            reminderTimer.Elapsed += ReminderTimer_Elapsed;

            EditCommand = new RelayCommand(
                i => navigationVM.NavigateToEventEditorViewToEdit(this));
            DeleteCommand = new RelayCommand(
                i => Delete());
        }

        /// <summary>
        /// Regenerate event cards and timers after a start/end time change.
        /// </summary>
        private void SaveTimeChanges()
        {
            CreateEventCardVMs();
            SetStatus();
        }

        /// <summary>
        /// Create event card view models for this event.
        /// </summary>
        private void CreateEventCardVMs()
        {
            EventCardVMsRemoved?.Invoke(this);
            EventCardVMs.Clear();

            int dayNum = (EndTime.Date - StartTime.Date).Days + 1;
            bool firstDay = true;
            for (int i = 1; i <= dayNum; i++)
            {
                DateTime dateOnCalendar = StartTime.Date.AddDays(i - 1);
                var cardVM = new CalendarEventCardVM()
                {
                    DateOnCalendar = dateOnCalendar,
                    TotalDayNum = dayNum,
                    EventVM = this,
                    IsFirstDay = firstDay
                };
                EventCardVMs.Add(dateOnCalendar, cardVM);
                firstDay = false;
            }

            EventCardVMsAdded?.Invoke(this);
        }

        /// <summary>
        /// Delete this event.
        /// </summary>
        private void Delete()
        {
            if (dialogService.ShowConfirmation("Bạn có thật sự muốn xóa loại sự kiện này?"))
                eventVMManager.Delete(this);
        }

        private void StatusUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SetStatus();
        }

        /// <summary>
        /// Set current status of this event.
        /// </summary>
        private void SetStatus()
        {
            if (DateTime.Now < StartTime)
            {
                StatusVM = CalendarEventStatuses.Upcoming;
                statusUpdateTimer.ElapsedTime = StartTime;
            }
            else
            {
                if (DateTime.Now < EndTime)
                {
                    StatusVM = CalendarEventStatuses.Happening;
                    statusUpdateTimer.ElapsedTime = EndTime;
                }
                else
                    StatusVM = CalendarEventStatuses.Finished;
            }
        }

        /// <summary>
        /// Set timer to remind again.
        /// </summary>
        /// <param name="interval">Remind again interval</param>
        public void RemindAgain(TimeSpan interval)
        {
            reminderTimer.ElapsedTime = DateTime.Now + interval;
            TimeSpan remainingTime = StartTime - reminderTimer.ElapsedTime;
            if (remainingTime < TimeSpan.Zero)
                RemainingTime = TimeSpan.Zero;
            else
                RemainingTime = remainingTime;
        }

        /// <summary>
        /// Set elapsed time on reminder timer.
        /// </summary>
        private void SetReminderTimer()
        {
            reminderTimer.ElapsedTime = StartTime - ReminderTime;
        }

        public void ReminderTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            navigationVM.NavigateToReminderView(this);

            string title;
            if (RemainingTime.Ticks == 0)
                title = $"Sự kiện \"{Subject}\" đã bắt đầu!";
            else
                title = $"Sự kiện \"{Subject}\" sắp diễn ra trong {TimeSpanToString.ConvertFromTimeSpan(RemainingTime)}nữa!";

            string subtitle = $"Bắt đầu: {StartTime:f}\nKết thúc: {EndTime:f}";
            new ToastContentBuilder().AddText(title).AddText(subtitle).AddText(Description).Show();
        }

        /// <summary>
        /// Dispose unmanaged resources of this event.
        /// </summary>
        public void Dispose()
        {
            statusUpdateTimer.Dispose();
        }
    }
}