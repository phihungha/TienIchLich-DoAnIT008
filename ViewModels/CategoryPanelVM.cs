using System.Collections.ObjectModel;
using System.Windows.Input;
using TienIchLich.Services;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model for the category panel.
    /// </summary>
    public class CategoryPanelVM : ViewModelBase
    {
        /// <summary>
        /// Calendar category view models to display in a DataGrid.
        /// </summary>
        public ObservableCollection<CalendarCategoryVM> CalendarCategoryVMs { get; private set; }

        /// <summary>
        /// Command to add a new category.
        /// </summary>
        public ICommand AddCommand { get; private set; }

        public CategoryPanelVM(CalendarCategoryVMManager categoryVMManager, DialogService dialogService)
        {
            CalendarCategoryVMs = categoryVMManager.CalendarCategoryVMs;
            AddCommand = new RelayCommand(
                i => categoryVMManager.Add(new CalendarCategoryVM(categoryVMManager, dialogService)));
        }
    }
}