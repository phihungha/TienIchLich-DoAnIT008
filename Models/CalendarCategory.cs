using System.Collections.Generic;

namespace TienIchLich.Models
{
    /// <summary>
    /// Data model for a calendar category.
    /// </summary>
    public class CalendarCategory
    {
        /// <summary>
        /// Id of this category in database. Primary key.
        /// </summary>
        public int CalendarCategoryId { get; set; }

        /// <summary>
        /// Name of this category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display color of events belong to this category.
        /// </summary>
        public string DisplayColor { get; set; }

        /// <summary>
        /// Events belong to this category.
        /// </summary>
        public List<CalendarEvent> Events { get; set; }
    }
}