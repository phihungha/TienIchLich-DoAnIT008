using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    public class CalendarVM : ViewModelBase
    {
        private ObservableCollection<CalendarEventVM> calendarEventVMs = new();
        private ObservableCollection<CalendarCategoryVM> calendarCategoryVMs = new();
        private DateTime? selectedDate;

        private ICommand addEventCommand = new RelayCommand(i => { });

        /// <summary>
        /// Calendar event view models to display.
        /// </summary>
        public ObservableCollection<CalendarEventVM> CalendarEventVMs => calendarEventVMs;

        /// <summary>
        /// Calendar category view models for MonthEventCalendar to monitor and update itself when
        /// a category's display box is checked.
        /// </summary>
        public ObservableCollection<CalendarCategoryVM> CalendarCategoryVMs => calendarCategoryVMs;

        /// <summary>
        /// Currently selected date on the calendar control.
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

        public CalendarVM(CalendarEventVMManager calendarEventVMs, ObservableCollection<CalendarCategoryVM> calendarCategoryVMs, NavigationVM navigationVM)
        {
            this.calendarEventVMs = calendarEventVMs.CalendarEventVMs;
            this.calendarCategoryVMs = calendarCategoryVMs;
            this.addEventCommand = new RelayCommand(
                i => navigationVM.NavigateToEventEditorViewOnAdd(this.SelectedDate));
        }
        
        public CalendarVM()
        {

        }
    }
}
