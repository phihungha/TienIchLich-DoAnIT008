using System.Collections.ObjectModel;

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
        /// View model for timeline view.
        /// </summary>
        public TimelineVM TimelineVM { get; private set; }

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

        public MainWorkspaceVM(ObservableCollection<CalendarEventVM> eventVMs, CalendarCategoryVMManager categoryVMManager, NavigationVM navigationVM)
        {
            CalendarVM = new CalendarVM(eventVMs, categoryVMManager.CalendarCategoryVMs, navigationVM, this);
            EventListVM = new EventListVM(eventVMs);
            UpcomingOverviewVM = new UpcomingOverviewVM(eventVMs);
            TimelineVM = new TimelineVM(eventVMs, categoryVMManager);
            StatisticsVM = new StatisticsVM(eventVMs, categoryVMManager.CalendarCategoryVMs);
        }
    }
}