using System.Collections.ObjectModel;
using System.Linq;
using TienIchLich.Models;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// Contains data and methods use by all other view models.
    /// Handles navigation.
    /// </summary>
    public class MasterViewModel : ViewModelBase
    {
        CalendarData calendarData;
        ViewModelBase currentViewModel;
        MainViewModel mainViewModel;
        EventEditorViewModel eventEditorViewModel;

        /// <summary>
        /// Current view model to display
        /// </summary>
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return currentViewModel;
            }
            set
            {
                currentViewModel = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Calendar data view model
        /// </summary>
        public CalendarData CalendarData
        {
            get
            {
                return calendarData;
            }
            set
            {
                calendarData = value;
                NotifyPropertyChanged();
            }
        }

        public MasterViewModel(CalendarData calendarData)
        {
            this.calendarData = calendarData;
            this.mainViewModel = new MainViewModel(this);
            this.eventEditorViewModel = new EventEditorViewModel(this);
            this.currentViewModel = this.eventEditorViewModel;
        }

        public void NavigateToMainView()
        {
            this.CurrentViewModel = this.mainViewModel;
        }
    }
}
