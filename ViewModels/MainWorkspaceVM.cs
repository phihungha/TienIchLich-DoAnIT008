using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model of the main workspace view.
    /// </summary>
    public class MainWorkspaceVM : ViewModelBase
    {
        /// <summary>
        /// View model for calendar view.
        /// </summary>
        public CalendarVM CalendarVM { get; private set; }

        /// <summary>
        /// View model for calendar event list view.
        /// </summary>
        public EventListVM EventListVM { get; private set; }

        /// <summary>
        /// View model for upcoming event overview.
        /// </summary>
        public UpcomingOverviewVM UpcomingOverviewVM { get; private set; }

        public MainWorkspaceVM(CalendarVM calendarVM, EventListVM eventListVM, UpcomingOverviewVM upcomingOverviewVM)
        {
            CalendarVM = calendarVM;
            EventListVM = eventListVM;
            UpcomingOverviewVM = upcomingOverviewVM;
        }
    }
}