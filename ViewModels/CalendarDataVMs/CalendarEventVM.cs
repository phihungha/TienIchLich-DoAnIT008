using System;
using System.Windows.Input;
using System.Timers;
using Microsoft.Toolkit.Uwp.Notifications;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model of a calendar event.
    /// </summary>
    public class CalendarEventVM : ViewModelBase
    {
        private Timer reminderTimer;

        private ICommand openEditorCommand;

        private int id = 0;
        private string subject = "(Tên rỗng)";
        private DateTime startTime = DateTime.Now.Date.AddDays(1);
        private DateTime endTime = DateTime.Now.Date.AddDays(2);
        private bool allDay = true;
        private TimeSpan reminderTime = new TimeSpan(0, 30, 0);
        private CalendarCategoryVM calendarCategoryVM;
        private string description = "";

        /// <summary>
        /// Command to open event editor in edit mode for this event.
        /// </summary>
        public ICommand OpenEditorCommand
        {
            get
            {
                return openEditorCommand;
            }
        }

        /// <summary>
        /// Id of event in the model for searching.
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

        /// <summary>
        /// Subject of event.
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

        /// <summary>
        /// Event start time.
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Event end time.
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                endTime = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Event happens in an entire day.
        /// Used for hiding hour:minutes textbox.
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

        /// <summary>
        /// Time to remind users before event starts.
        /// </summary>
        public TimeSpan ReminderTime
        {
            get
            {
                return reminderTime;
            }
            set
            {
                reminderTime = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Description of event.
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

        /// <summary>
        /// View model for the calendar category of event.
        /// </summary>
        public CalendarCategoryVM CalendarCategoryVM 
        {
            get
            {
                return calendarCategoryVM;
            }
            set
            {
                calendarCategoryVM = value;
                NotifyPropertyChanged();
            }
        }

        public CalendarEventVM(NavigationVM navigationVM, DateTime? startTime = null)
        {
            if (startTime != null)
            {
                this.StartTime = (DateTime)startTime;
                this.EndTime = this.StartTime.AddDays(1);
            }
            
            this.openEditorCommand = new RelayCommand(
                i => navigationVM.NavigateToEventEditorViewOnEdit(this),
                i => true);

            this.reminderTimer = new Timer();
            this.reminderTimer.AutoReset = false;
            this.reminderTimer.Elapsed += ReminderTimer_Elapsed;
        }

        public void StartReminderTimer()
        {
            double timeUntilEventStarts = (this.StartTime - this.ReminderTime - DateTime.Now).TotalMilliseconds;
            if (timeUntilEventStarts > 0)
            {
                this.reminderTimer.Interval = timeUntilEventStarts;
                this.reminderTimer.Enabled = true;
            }
            else
                this.reminderTimer.Enabled = false;
        }

        public void StopReminderTimer()
        {
            this.reminderTimer.Dispose();
        }

        private void ReminderTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            new ToastContentBuilder()
                .AddText($"Sự kiện {this.Subject} sắp diễn ra trong {this.ReminderTime} nữa!")
                .AddText($"Bắt đầu: {this.StartTime.ToString("F")}\nKết thúc: {this.EndTime.ToString("F")}")
                .AddText(this.Description).Show();
            this.reminderTimer.Enabled = false;
        }
    }
}
