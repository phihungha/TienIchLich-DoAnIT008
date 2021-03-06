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
        public NavigationVM NavigationVM { get; private set; } = new();

        /// <summary>
        /// View model for category panel.
        /// </summary>
        public SidePanelVM SidePanelVM { get; private set; }

        public MainWindowVM(DialogService dialogService, AlarmSoundService alarmSoundService)
        {
            var calendarCategoryVMManager = new CalendarCategoryVMManager(dialogService);
            var calendarEventVMManager = new CalendarEventVMManager(NavigationVM, calendarCategoryVMManager, dialogService);
            calendarCategoryVMManager.EventVMManager = calendarEventVMManager;

            var settingsVM = new SettingsVM(NavigationVM, alarmSoundService, dialogService);

            var mainWorkspaceVM = new MainWorkspaceVM(calendarEventVMManager.CalendarEventVMs, calendarCategoryVMManager, NavigationVM);

            var categoryPanelVM = new CategoryPanelVM(calendarCategoryVMManager, dialogService);
            SidePanelVM = new SidePanelVM(NavigationVM, categoryPanelVM, mainWorkspaceVM.CalendarVM);

            var eventEditorVM = new EventEditorVM(NavigationVM, dialogService, calendarEventVMManager, calendarCategoryVMManager.CalendarCategoryVMs);
            var reminderVM = new ReminderVM(NavigationVM, SidePanelVM, alarmSoundService);

            NavigationVM.EventEditorVM = eventEditorVM;
            NavigationVM.MainWorkspaceVM = mainWorkspaceVM;
            NavigationVM.ReminderVM = reminderVM;
            NavigationVM.SettingsVM = settingsVM;
            NavigationVM.DisplayedVM = mainWorkspaceVM;
        }
    }
}