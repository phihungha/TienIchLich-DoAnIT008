using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model of the main workspace view.
    /// </summary>
    public class MainWorkspaceVM : ViewModelBase
    {
        /// <summary>
        /// Command to add a new event.
        /// </summary>
        public ICommand AddEventCommand { get; private set; }

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

        public MainWorkspaceVM(CalendarEventVMManager calendarEventVMManager, CalendarCategoryVMManager calendarCategoryVMManager, NavigationVM navigationVM)
        {
            CalendarVM = new CalendarVM(calendarEventVMManager.CalendarEventVMs, calendarCategoryVMManager.CalendarCategoryVMs, navigationVM);
            EventListVM = new EventListVM(calendarEventVMManager.CalendarEventVMs);
            UpcomingOverviewVM = new UpcomingOverviewVM(calendarEventVMManager.CalendarEventVMs);
            AddEventCommand = new RelayCommand(
                i => navigationVM.NavigateToEventEditorViewToAdd(CalendarVM.SelectedDate)
                );
        }
    }
}