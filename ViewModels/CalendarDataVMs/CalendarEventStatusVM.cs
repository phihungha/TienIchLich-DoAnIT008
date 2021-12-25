namespace TienIchLich.ViewModels
{
    /// <summary>
    /// Available calendar event statuses to use.
    /// </summary>
    public static class CalendarEventStatuses
    {
        public readonly static CalendarEventStatusVM Upcoming = new() { Id = CalendarEventStatusId.Upcoming };
        public readonly static CalendarEventStatusVM Happening = new() { Id = CalendarEventStatusId.Happening };
        public readonly static CalendarEventStatusVM Finished = new() { Id = CalendarEventStatusId.Finished };
    }

    /// <summary>
    /// Statuses of an event.
    /// </summary>
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
        /// This status's identifier.
        /// </summary>
        public CalendarEventStatusId Id { get; set; }

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
