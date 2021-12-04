namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model of the main window.
    /// </summary>
    public class MainWindowVM : ViewModelBase
    {
        CategoryPanelVM categoryPanelVM;
        NavigationVM navigationVM;

        /// <summary>
        /// Current view model to display
        /// </summary>
        public NavigationVM NavigationVM => navigationVM;

        public CategoryPanelVM CategoryPanelVM => categoryPanelVM;

        public MainWindowVM()
        {
            this.navigationVM = new NavigationVM();
            var calendarCategoryVMManager = new CalendarCategoryVMManager();
            var calendarEventVMManager = new CalendarEventVMManager(this.navigationVM, calendarCategoryVMManager);

            this.categoryPanelVM = new CategoryPanelVM(calendarCategoryVMManager);

            var mainWorkspaceVM = new MainWorkspaceVM(calendarEventVMManager, calendarCategoryVMManager, this.navigationVM);
            var eventEditorVM = new EventEditorVM(this.navigationVM, calendarEventVMManager, calendarCategoryVMManager.CalendarCategoryVMs);

            this.navigationVM.EventEditorVM = eventEditorVM;
            this.navigationVM.MainWorkspaceVM = mainWorkspaceVM;
            this.navigationVM.DisplayedVM = mainWorkspaceVM;
        }
    }
}
