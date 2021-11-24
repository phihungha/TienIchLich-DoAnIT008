using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TienIchLich
{
    /// <summary>
    /// View model of a calendar event.
    /// </summary>
    public class CalendarEventViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string subject = "(No subject)";
        DateTime startTime;
        DateTime endTime;
        bool allDay = true;
        TimeSpan reminderTime = new TimeSpan(0, 30, 0);
        CalendarCategoryViewModel category;
        string description = "";

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
        /// Calendar category of event.
        /// </summary>
        public CalendarCategoryViewModel Category 
        {
            get
            {
                return category;
            }
            set
            {
                category = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Raises property change event.
        /// </summary>
        /// <param name="propertyName"></param>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public CalendarEventViewModel()
        {
            // Set default start and end date.
            DateTime currentTime = DateTime.Now;
            startTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day);
            startTime = startTime.AddDays(1);
            endTime = startTime.AddDays(1);
        }
    }
}
