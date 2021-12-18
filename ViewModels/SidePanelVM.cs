using System;
using System.Globalization;
using System.Timers;
using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model for side panel.
    /// </summary>
    public class SidePanelVM : ViewModelBase
    {
        Timer currentTimeUpdateTimer = new() { Interval = 1000, Enabled = true };
        private DateTime currentTime = DateTime.Now;

        /// <summary>
        /// Current time to display.
        /// </summary>
        public DateTime CurrentTime
        {
            get
            {
                return currentTime;
            }
            set
            {
                currentTime = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// View model for calendar category panel.
        /// </summary>
        public CategoryPanelVM CategoryPanelVM { get; private set; }

        /// <summary>
        /// Command to add a new event.
        /// </summary>
        public ICommand AddEventCommand { get; private set; }

        /// <summary>
        /// Command to open settings view.
        /// </summary>
        public ICommand OpenSettingsCommand { get; private set; }

        public SidePanelVM(NavigationVM navigationVM, CategoryPanelVM categoryPanelVM, CalendarVM calendarVM)
        {
            currentTimeUpdateTimer.Elapsed += CurrentTimeUpdateTimer_Elapsed;
            CategoryPanelVM = categoryPanelVM;
            AddEventCommand = new RelayCommand(
                i => navigationVM.NavigateToEventEditorViewToAdd(calendarVM.MonthCalendarSelectedDate));
            OpenSettingsCommand = new RelayCommand(
                i => navigationVM.NavigateToSettingsView());
        }

        private void CurrentTimeUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CurrentTime = DateTime.Now;
        }
    }
}
