using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model for a calendar view.
    /// </summary>
    public class CalendarVM : ViewModelBase
    {
        private MainWorkspaceVM mainWorkspaceVM;

        /// <summary>
        /// Calendar event view models to display.
        /// </summary>
        public ObservableCollection<CalendarEventVM> CalendarEventVMs { get; private set; }

        /// <summary>
        /// Calendar category view models for the calendar control to monitor
        /// and update itself when they change.
        /// </summary>
        public ObservableCollection<CalendarCategoryVM> CalendarCategoryVMs { get; private set; }

        private DateTime? monthCalendarSelectedDate = DateTime.Now;

        /// <summary>
        /// Selected date on the calendar control.
        /// </summary>
        public DateTime? MonthCalendarSelectedDate
        {
            get
            {
                return monthCalendarSelectedDate;
            }
            set
            {
                monthCalendarSelectedDate = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime monthCalendarDisplayDate = DateTime.Now;

        /// <summary>
        /// Display date on the calendar control.
        /// </summary>
        public DateTime MonthCalendarDisplayDate
        {
            get
            {
                return monthCalendarDisplayDate;
            }
            set
            {
                monthCalendarDisplayDate = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime? yearCalendarSelectedDate = DateTime.Now;

        public DateTime? YearCalendarSelectedDate
        {
            get
            {
                return yearCalendarSelectedDate;
            }
            set
            {
                yearCalendarSelectedDate = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Command to add new calendar event.
        /// </summary>
        public ICommand AddEventCommand { get; private set; }

        /// <summary>
        /// Command to jump to today's date and select it.
        /// </summary>
        public ICommand TodayCommand { get; private set; }

        /// <summary>
        /// Command to jump to a different month on month calendar from year calendar.
        /// </summary>
        public ICommand JumpToAnotherMonthCommand { get; private set; }

        public CalendarVM(ObservableCollection<CalendarEventVM> eventVMs, ObservableCollection<CalendarCategoryVM> categoryVMs, NavigationVM navigationVM, MainWorkspaceVM mainWorkspaceVM)
        {
            this.mainWorkspaceVM = mainWorkspaceVM;
            CalendarEventVMs = eventVMs;
            CalendarCategoryVMs = categoryVMs;
            AddEventCommand = new RelayCommand(
                i => navigationVM.NavigateToEventEditorViewToAdd(MonthCalendarSelectedDate));
            TodayCommand = new RelayCommand(i => JumpToToday());
            JumpToAnotherMonthCommand = new RelayCommand(i => JumpToAnotherMonth());
        }

        /// <summary>
        /// Jump to current date and select it.
        /// </summary>
        private void JumpToToday()
        {
            MonthCalendarDisplayDate = DateTime.Now;
            MonthCalendarSelectedDate = DateTime.Now;
        }

        /// <summary>
        /// Jump to another month on month calendar.
        /// </summary>
        private void JumpToAnotherMonth()
        {
            if (YearCalendarSelectedDate != null)
            {
                MonthCalendarDisplayDate = (DateTime)YearCalendarSelectedDate;
                MonthCalendarSelectedDate = YearCalendarSelectedDate;
                mainWorkspaceVM.SelectedTabIndex = 1;
                Thread.Sleep(200);
            }
        }
    }
}