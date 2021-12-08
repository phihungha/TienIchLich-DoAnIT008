using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model of the main workspace.
    /// </summary>
    public class MainWorkspaceVM : ViewModelBase
    {
        ICommand addEventCommand;

        /// <summary>
        /// Command to add a new event.
        /// </summary>
        public ICommand AddEventCommand => addEventCommand;

        /// <summary>
        /// View model for calendar controls.
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
            this.CalendarVM = new CalendarVM(calendarEventVMManager, calendarCategoryVMManager.CalendarCategoryVMs, navigationVM);
            this.EventListVM = new EventListVM(calendarEventVMManager.CalendarEventVMs);
            this.UpcomingOverviewVM = new UpcomingOverviewVM(calendarEventVMManager.CalendarEventVMs);
            this.addEventCommand = new RelayCommand(
                i => navigationVM.NavigateToEventEditorViewOnAdd(CalendarVM.SelectedDate)
                );
        }
    }
}
