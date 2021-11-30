using System.Collections.Generic;

namespace TienIchLich.Models
{
    /// <summary>
    /// A calendar category to group events.
    /// </summary>
    public class CalendarCategory
    {
        /// <summary>
        /// Id of the category in database. Primary key.
        /// </summary>
        public int CalendarCategoryId { get; set; }

        /// <summary>
        /// Category name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display color of events associate with this category.
        /// </summary>
        public string DisplayColor { get; set; }

        /// <summary>
        /// Events associate with this category.
        /// </summary>
        public List<CalendarEvent> Events { get; set; }
    }
}
