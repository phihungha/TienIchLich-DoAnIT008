using System.Collections.ObjectModel;

namespace TienIchLich.ViewModels
{
    enum CategoryColorOption
    {
        Red,
        Yellow,
        Green,
        Blue,
        Cyan,
        Purple,
        Black,
        Custom
    }

    public class CategoryPanelVM : ViewModelBase
    {
        private CalendarCategoryVMs calendarCategoryVMs;

        public ObservableCollection<CalendarCategoryVM> CalendarCategories 
        { get => calendarCategoryVMs.CategoryVMs; }

        public CategoryPanelVM(CalendarCategoryVMs calendarCategoryVMs)
        {
            this.calendarCategoryVMs = calendarCategoryVMs;
        }
    }
}
