using System;

namespace TienIchLich.Models
{
    /// <summary>
    /// A calendar event.
    /// </summary>
    public class CalendarEvent
    {
        /// <summary>
        /// Id of event in database. Primary key.
        /// </summary>
        public int CalendarEventId { get; set; }

        /// <summary>
        /// Title of event.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Event start time.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Event end time.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Event happens in an entire day.
        /// </summary>
        public bool AllDay { get; set; }

        /// <summary>
        /// Time to remind users before event starts.
        /// </summary>
        public TimeSpan ReminderTime { get; set; }

        /// <summary>
        /// Description of event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Calendar category Id of event. Foreign key.
        /// </summary>
        public int CalendarCategoryId { get; set; } // Khóa ngoại

        /// <summary>
        /// Instance of the calendar category of event.
        /// </summary>
        public CalendarCategory CalendarCategory { get; set; }
    }
}
