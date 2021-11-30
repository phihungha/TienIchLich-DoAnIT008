using System.Collections.ObjectModel;
using System.Linq;
using TienIchLich.Models;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model of the main workspace.
    /// </summary>
    public class MainWorkspaceVM : ViewModelBase
    {
        private CalendarEventVMs calendarEventVMs;

        /// <summary>
        /// Calendar event view models to display.
        /// </summary>
        public ObservableCollection<CalendarEventVM> CalendarEvents => calendarEventVMs.EventVMs;

        public MainWorkspaceVM(CalendarEventVMs calendarEventVMs)
        {
            this.calendarEventVMs = calendarEventVMs;
        }
    }
}
