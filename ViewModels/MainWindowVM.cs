using TienIchLich.Models;
using TienIchLich.Services;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model of the main window.
    /// </summary>
    public class MainWindowVM : ViewModelBase
    {
        /// <summary>
        /// View model for navigation purposes.
        /// </summary>
        public NavigationVM NavigationVM { get; private set; }

        /// <summary>
        /// View model for category panel.
        /// </summary>
        public SidePanelVM SidePanelVM { get; private set; }

        public MainWindowVM(DialogService dialogService, AlarmSoundService alarmSoundService)
        {
            NavigationVM = new NavigationVM();

            var calendarCategoryVMManager = new CalendarCategoryVMManager(dialogService);
            var reminderManager = new ReminderManager();
            var calendarEventVMManager = new CalendarEventVMManager(NavigationVM, calendarCategoryVMManager, reminderManager, dialogService);
            calendarCategoryVMManager.EventVMManager = calendarEventVMManager;

            var settingsVM = new SettingsVM(NavigationVM, alarmSoundService, dialogService);
            var calendarVM = new CalendarVM(calendarEventVMManager.CalendarEventVMs, calendarCategoryVMManager.CalendarCategoryVMs, NavigationVM);
            var eventListVM = new EventListVM(calendarEventVMManager.CalendarEventVMs);
            var upcomingOverviewVM = new UpcomingOverviewVM(calendarEventVMManager.CalendarEventVMs);
            var statisticsVM = new StatisticsVM(calendarEventVMManager.CalendarEventVMs, calendarCategoryVMManager.CalendarCategoryVMs);
            var mainWorkspaceVM = new MainWorkspaceVM(calendarVM, eventListVM, upcomingOverviewVM, statisticsVM);

            var eventEditorVM = new EventEditorVM(NavigationVM, dialogService, calendarEventVMManager, calendarCategoryVMManager.CalendarCategoryVMs);
            var reminderVM = new ReminderVM(NavigationVM, reminderManager, alarmSoundService);

            var categoryPanelVM = new CategoryPanelVM(calendarCategoryVMManager, dialogService);
            SidePanelVM = new SidePanelVM(NavigationVM, categoryPanelVM, calendarVM);

            NavigationVM.EventEditorVM = eventEditorVM;
            NavigationVM.MainWorkspaceVM = mainWorkspaceVM;
            NavigationVM.ReminderVM = reminderVM;
            NavigationVM.SettingsVM = settingsVM;
            NavigationVM.DisplayedVM = mainWorkspaceVM;
        }
    }
}