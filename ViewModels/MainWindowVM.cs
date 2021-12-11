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

        public MainWindowVM(DialogService dialogService)
        {
            NavigationVM = new NavigationVM();

            var calendarCategoryVMManager = new CalendarCategoryVMManager(dialogService);
            var calendarEventVMManager = new CalendarEventVMManager(NavigationVM, calendarCategoryVMManager);

            CategoryPanelVM = new CategoryPanelVM(calendarCategoryVMManager, dialogService);
            var mainWorkspaceVM = new MainWorkspaceVM(calendarEventVMManager, calendarCategoryVMManager, NavigationVM);
            var eventEditorVM = new EventEditorVM(NavigationVM, dialogService, calendarEventVMManager, calendarCategoryVMManager.CalendarCategoryVMs);
            var reminderVM = new ReminderVM(NavigationVM);
            NavigationVM.EventEditorVM = eventEditorVM;
            NavigationVM.MainWorkspaceVM = mainWorkspaceVM;
            NavigationVM.DisplayedVM = reminderVM;
            reminderVM.EventVM = calendarEventVMManager.CalendarEventVMs[0];
        }
    }
}
