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

        /// <summary>
        /// View model for upcoming event overview.
        /// </summary>
        public StatisticsVM StatisticsVM { get; private set; }

        private int selectedTabIndex = 1;

        /// <summary>
        /// Currently selected tab index on the workspace.
        /// </summary>
        public int SelectedTabIndex
        {
            get
            {
                return selectedTabIndex;
            }
            set
            {
                selectedTabIndex = value;
                NotifyPropertyChanged();
            }
        }

        public MainWorkspaceVM(CalendarVM calendarVM, EventListVM eventListVM, UpcomingOverviewVM upcomingOverviewVM, StatisticsVM statisticsVM)
        {
            CalendarVM = calendarVM;
            EventListVM = eventListVM;
            UpcomingOverviewVM = upcomingOverviewVM;
            StatisticsVM = statisticsVM;
        }
    }
}