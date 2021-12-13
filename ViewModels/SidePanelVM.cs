using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model for side panel.
    /// </summary>
    public class SidePanelVM : ViewModelBase
    {
        /// <summary>
        /// View model for calendar category panel.
        /// </summary>
        public CategoryPanelVM CategoryPanelVM { get; private set; }

        /// <summary>
        /// Command to add a new event.
        /// </summary>
        public ICommand AddEventCommand { get; private set; }

        /// <summary>
        /// Command to open settings view.
        /// </summary>
        public ICommand OpenSettingsCommand { get; private set; }

        public SidePanelVM(NavigationVM navigationVM, CategoryPanelVM categoryPanelVM, CalendarVM calendarVM)
        {
            CategoryPanelVM = categoryPanelVM;
            AddEventCommand = new RelayCommand(
                i => navigationVM.NavigateToEventEditorViewToAdd(calendarVM.SelectedDate));
            OpenSettingsCommand = new RelayCommand(
                i => navigationVM.NavigateToSettingsView());
        }
    }
}
