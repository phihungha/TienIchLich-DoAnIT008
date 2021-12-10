using System;

namespace TienIchLich.Models
{
    /// <summary>
    /// Data model for a calendar event.
    /// </summary>
    public class CalendarEvent
    {
        /// <summary>
        /// Id of this event in database. Primary key.
        /// </summary>
        public int CalendarEventId { get; set; }

        /// <summary>
        /// Subject of this event.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// This event's start time.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// This event's end time.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Does this event happen in an entire day.
        /// </summary>
        public bool AllDay { get; set; }

        /// <summary>
        /// Time to remind the user before this event starts.
        /// </summary>
        public TimeSpan ReminderTime { get; set; }

        /// <summary>
        /// Description of this event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Id of the calendar category this event belongs to. Foreign key.
        /// </summary>
        public int CalendarCategoryId { get; set; } // Foreign key

        /// <summary>
        /// Instance of the calendar category this event belongs to.
        /// </summary>
        public CalendarCategory CalendarCategory { get; set; }
    }
}