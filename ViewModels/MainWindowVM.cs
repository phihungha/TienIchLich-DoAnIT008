namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model of the main window and the entire application GUI.
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
            var calendarCategoryVMs = new CalendarCategoryVMs();
            var calendarEventVMs = new CalendarEventVMs(this.navigationVM, calendarCategoryVMs);

            this.categoryPanelVM = new CategoryPanelVM(calendarCategoryVMs);

            var mainWorkspaceVM = new MainWorkspaceVM(calendarEventVMs, this.navigationVM);
            var eventEditorVM = new EventEditorVM(this.navigationVM, calendarEventVMs, calendarCategoryVMs);
            this.navigationVM.EventEditorVM = eventEditorVM;
            this.navigationVM.MainWorkspaceVM = mainWorkspaceVM;
            this.navigationVM.DisplayedVM = mainWorkspaceVM;
        }
    }
}
