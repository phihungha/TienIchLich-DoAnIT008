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
        public CategoryPanelVM CategoryPanelVM { get; private set; }

        public MainWindowVM(DialogService dialogService, AlarmSoundService alarmSoundService)
        {
            NavigationVM = new NavigationVM();

            var calendarCategoryVMManager = new CalendarCategoryVMManager(dialogService);
            var reminderManager = new ReminderManager();
            var calendarEventVMManager = new CalendarEventVMManager(NavigationVM, calendarCategoryVMManager, reminderManager);
            calendarCategoryVMManager.EventVMManager = calendarEventVMManager;

            var settingsVM = new SettingsVM(NavigationVM, alarmSoundService, dialogService);

            CategoryPanelVM = new CategoryPanelVM(calendarCategoryVMManager, dialogService);
            var mainWorkspaceVM = new MainWorkspaceVM(calendarEventVMManager, calendarCategoryVMManager, NavigationVM);
            var eventEditorVM = new EventEditorVM(NavigationVM, dialogService, calendarEventVMManager, calendarCategoryVMManager.CalendarCategoryVMs);
            var reminderVM = new ReminderVM(NavigationVM, reminderManager, alarmSoundService);

            NavigationVM.EventEditorVM = eventEditorVM;
            NavigationVM.MainWorkspaceVM = mainWorkspaceVM;
            NavigationVM.ReminderVM = reminderVM;
            NavigationVM.SettingsVM = settingsVM;
            NavigationVM.DisplayedVM = mainWorkspaceVM;
        }
    }
}