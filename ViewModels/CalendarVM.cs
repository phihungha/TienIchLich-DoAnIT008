using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    public class CalendarVM : ViewModelBase
    {
        private ObservableCollection<CalendarEventVM> calendarEvents = new();
        private DateTime? selectedDate;

        private ICommand addEventCommand = new RelayCommand(i => { });

        /// <summary>
        /// Calendar event view models to display.
        /// </summary>
        public ObservableCollection<CalendarEventVM> CalendarEvents => calendarEvents;

        /// <summary>
        /// Currently selected date.
        /// </summary>
        public DateTime? SelectedDate
        {
            get
            {
                return selectedDate;
            }
            set
            {
                selectedDate = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Command to add new calendar event.
        /// </summary>
        public ICommand AddEventCommand => addEventCommand;

        public CalendarVM(CalendarEventVMs calendarEventVMs, NavigationVM navigationVM)
        {
            this.calendarEvents = calendarEventVMs.EventVMs;
            this.addEventCommand = new RelayCommand(
                i => navigationVM.NavigateToEventEditorViewOnAdd(this.SelectedDate));
        }
        
        public CalendarVM()
        {

        }
    }
}
