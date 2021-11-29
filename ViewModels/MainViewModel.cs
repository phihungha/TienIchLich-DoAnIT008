using System.Collections.ObjectModel;
using System.Linq;
using TienIchLich.Models;

namespace TienIchLich.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<CalendarEventViewModel> calendarEvents;

        /// <summary>
        /// Calendar events.
        /// </summary>
        public ObservableCollection<CalendarEventViewModel> CalendarEvents
        {
            get
            {
                return calendarEvents;
            }
            set
            {
                calendarEvents = value;
                NotifyPropertyChanged();
            }
        }

        public MainViewModel(MasterViewModel masterVM)
        {
            this.CalendarEvents = masterVM.CalendarData.CalendarEvents;
        }
    }
}
