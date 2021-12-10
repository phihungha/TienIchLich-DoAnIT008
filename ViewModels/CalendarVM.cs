using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model for a calendar view.
    /// </summary>
    public class CalendarVM : ViewModelBase
    {
        /// <summary>
        /// Calendar event view models to display.
        /// </summary>
        public ObservableCollection<CalendarEventVM> CalendarEventVMs { get; private set; }

        /// <summary>
        /// Calendar category view models for the calendar control to monitor
        /// and update itself when they change.
        /// </summary>
        public ObservableCollection<CalendarCategoryVM> CalendarCategoryVMs { get; private set; }

        private DateTime? selectedDate;

        /// <summary>
        /// Selected date on the calendar control.
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
        public ICommand AddEventCommand { get; private set; }

        public CalendarVM(ObservableCollection<CalendarEventVM> eventVMs, ObservableCollection<CalendarCategoryVM> categoryVMs, NavigationVM navigationVM)
        {
            CalendarEventVMs = eventVMs;
            CalendarCategoryVMs = categoryVMs;
            AddEventCommand = new RelayCommand(
                i => navigationVM.NavigateToEventEditorViewToAdd(this.SelectedDate)
                );
        }
    }
}