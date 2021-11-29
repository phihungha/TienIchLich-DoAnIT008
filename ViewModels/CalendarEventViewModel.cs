using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TienIchLich.ViewModels;

namespace TienIchLich
{
    /// <summary>
    /// View model of a calendar event.
    /// </summary>
    public class CalendarEventViewModel : ViewModelBase
    {
        int id = 0;
        string subject = "(No subject)";
        DateTime startTime;
        DateTime endTime;
        bool allDay = true;
        TimeSpan reminderTime = new TimeSpan(0, 30, 0);
        CalendarCategoryViewModel calendarCategory;
        string description = "";

        /// <summary>
        /// Id of event (For searching).
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
        public CalendarCategoryViewModel CalendarCategory 
        {
            get
            {
                return calendarCategory;
            }
            set
            {
                calendarCategory = value;
                NotifyPropertyChanged();
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
