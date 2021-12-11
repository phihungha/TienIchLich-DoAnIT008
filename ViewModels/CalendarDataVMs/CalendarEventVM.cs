using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Timers;
using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model of a calendar event.
    /// </summary>
    public class CalendarEventVM : ViewModelBase
    {
        NavigationVM navigationVM;

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
                startTime = value;
                NotifyPropertyChanged();
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
                endTime = value;
                NotifyPropertyChanged();
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
                reminderTime = value;
                NotifyPropertyChanged();
            }
        }

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
                categoryVM = value;
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

        public CalendarEventVM(CalendarEventVMManager eventVMManager, NavigationVM navigationVM, DateTime? startTime = null)
        {
            this.navigationVM = navigationVM;

            if (startTime != null)
            {
                StartTime = (DateTime)startTime;
                EndTime = StartTime.AddDays(1);
            }

            EditCommand = new RelayCommand(
                i => navigationVM.NavigateToEventEditorViewToEdit(this));
            DeleteCommand = new RelayCommand(
                i => eventVMManager.Delete(this));
        }

        public void ReminderTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            navigationVM.NavigateToReminderView(this);

            string title;
            if (ReminderTime.Ticks == 0)
                title = $"Sự kiện \"{Subject}\" đã bắt đầu!";
            else
                title = $"Sự kiện \"{Subject}\" sắp diễn ra trong {ReminderTime} nữa!";

            string subtitle = $"Bắt đầu: {StartTime.ToString("F")}\nKết thúc: {EndTime.ToString("F")}";
            new ToastContentBuilder().AddText(title).AddText(subtitle).AddText(Description).Show();
        }
    }
}