namespace TienIchLich.ViewModels
{
    /// <summary>
    /// Available calendar event statuses to use.
    /// </summary>
    public static class CalendarEventStatuses
    {
        public readonly static CalendarEventStatusVM Upcoming = new() 
        { 
            Id = CalendarEventStatusId.Upcoming, 
            DisplayColor = "#ff0000" 
        };
        public readonly static CalendarEventStatusVM Happening = new() 
        { 
            Id = CalendarEventStatusId.Happening, 
            DisplayColor = "#00e013" 
        };
        public readonly static CalendarEventStatusVM Finished = new() 
        { 
            Id = CalendarEventStatusId.Finished, 
            DisplayColor = "#00aaff" 
        };
    }

    public enum CalendarEventStatusId
    {
        Upcoming,
        Happening,
        Finished
    }

    /// <summary>
    /// View model for the status of a calendar event.
    /// </summary>
    public class CalendarEventStatusVM : ViewModelBase
    {
        /// <summary>
        /// Identifier of this status.
        /// </summary>
        public CalendarEventStatusId Id { get; set; }

        /// <summary>
        /// Display color of this status.
        /// </summary>
        public string DisplayColor { get; set; }

        private bool isDisplayed = true;

        /// <summary>
        /// True if events of this status is displayed.
        /// </summary>
        public bool IsDisplayed
        {
            get
            {
                return isDisplayed;
            }
            set
            {
                isDisplayed = value;
                NotifyPropertyChanged();
            }
        }
    }
}
