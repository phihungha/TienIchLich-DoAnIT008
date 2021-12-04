using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    public class CalendarVM : ViewModelBase
    {
        private ObservableCollection<CalendarEventVM> calendarEvents = new();
        private ObservableCollection<CalendarCategoryVM> calendarCategories = new();
        private DateTime? selectedDate;

        private ICommand addEventCommand = new RelayCommand(i => { });

        /// <summary>
        /// Calendar event view models to display.
        /// </summary>
        public ObservableCollection<CalendarEventVM> CalendarEvents => calendarEvents;

        /// <summary>
        /// Calendar category view models for MonthEventCalendar to monitor and update itself when
        /// a category's display box is checked.
        /// </summary>
        public ObservableCollection<CalendarCategoryVM> CalendarCategories => calendarCategories;

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

        public CalendarVM(CalendarEventVMs calendarEventVMs, CalendarCategoryVMs calendarCategoryVMs, NavigationVM navigationVM)
        {
            this.calendarEvents = calendarEventVMs.EventVMs;
            this.calendarCategories = calendarCategoryVMs.CategoryVMs;
            this.addEventCommand = new RelayCommand(
                i => navigationVM.NavigateToEventEditorViewOnAdd(this.SelectedDate));
        }
        
        public CalendarVM()
        {

        }
    }
}
