using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model of the main workspace.
    /// </summary>
    public class MainWorkspaceVM : ViewModelBase
    {
        private CalendarVM calendarVM;

        ICommand addEventCommand;

        /// <summary>
        /// Command to add a new event.
        /// </summary>
        public ICommand AddEventCommand => addEventCommand;

        /// <summary>
        /// View model for calendar controls.
        /// </summary>
        public CalendarVM CalendarVM => calendarVM; 
        
        public MainWorkspaceVM(CalendarEventVMs calendarEventVMs, CalendarCategoryVMs calendarCategoryVMs, NavigationVM navigationVM)
        {
            this.calendarVM = new CalendarVM(calendarEventVMs, calendarCategoryVMs, navigationVM);
            this.addEventCommand = new RelayCommand(
                i => navigationVM.NavigateToEventEditorViewOnAdd(CalendarVM.SelectedDate)
                );
        }
    }
}
