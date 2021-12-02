using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TienIchLich.ViewModels
{

    public class CategoryPanelVM : ViewModelBase
    {
        private CalendarCategoryVMs calendarCategoryVMs;

        private ICommand addCommand;

        public ObservableCollection<CalendarCategoryVM> CalendarCategories 
        { get => calendarCategoryVMs.CategoryVMs; }

        public ICommand AddCommand => addCommand;

        public CategoryPanelVM(CalendarCategoryVMs calendarCategoryVMs)
        {
            this.calendarCategoryVMs = calendarCategoryVMs;
            this.addCommand = new RelayCommand(
                i => calendarCategoryVMs.AddCalendarCategory(new CalendarCategoryVM(calendarCategoryVMs)), 
                i => true);
        }
    }
}
