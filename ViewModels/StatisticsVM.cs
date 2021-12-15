using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace TienIchLich.ViewModels
{
    public class StatisticsVM : ViewModelBase
    {
        private ObservableCollection<CalendarEventVM> eventVMs;
        private ObservableCollection<CalendarCategoryVM> categoryVMs;

        public StatisticsVM(ObservableCollection<CalendarEventVM> eventVMs, ObservableCollection<CalendarCategoryVM> categoryVMs)
        {
            this.eventVMs = eventVMs;
            this.categoryVMs = categoryVMs;
        }
    }
}
