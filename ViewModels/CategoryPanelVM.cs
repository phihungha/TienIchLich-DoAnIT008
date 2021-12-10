using System.Collections.ObjectModel;
using System.Windows.Input;
using TienIchLich.Services;

namespace TienIchLich.ViewModels
{
    public class CategoryPanelVM : ViewModelBase
    {
        private CalendarCategoryVMManager calendarCategoryVMManager;

        private ICommand addCommand;

        /// <summary>
        /// Calendar category view models to display in ItemsControl.
        /// </summary>
        public ObservableCollection<CalendarCategoryVM> CalendarCategoryVMs 
        { get => calendarCategoryVMManager.CalendarCategoryVMs; }

        /// <summary>
        /// Command to add a new category.
        /// </summary>
        public ICommand AddCommand => addCommand;

        public CategoryPanelVM(CalendarCategoryVMManager calendarCategoryVMManager, DialogService dialogService)
        {
            this.calendarCategoryVMManager = calendarCategoryVMManager;
            this.addCommand = new RelayCommand(
                i => calendarCategoryVMManager.AddCalendarCategory(new CalendarCategoryVM(calendarCategoryVMManager, dialogService)), 
                i => true);
        }
    }
}
