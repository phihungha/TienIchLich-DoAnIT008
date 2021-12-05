using System;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model to use in navigating between main workspace and event editor.
    /// </summary>
    public class NavigationVM : ViewModelBase
    {
        private MainWorkspaceVM mainWorkspaceVM;
        private EventEditorVM eventEditorVM;
        private ViewModelBase displayedVM;

        /// <summary>
        /// Currently displayed view model.
        /// </summary>
        public ViewModelBase DisplayedVM
        {
            get
            {
                return displayedVM;
            }
            set
            {
                displayedVM = value;
                NotifyPropertyChanged();
            }
        }

        public MainWorkspaceVM MainWorkspaceVM
        {
            set
            {
                mainWorkspaceVM = value;
                NotifyPropertyChanged();
            }
        }

        public EventEditorVM EventEditorVM
        {
            set
            {
                eventEditorVM = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Navigate to main workspace view
        /// </summary>
        public void NavigateToMainWorkspaceView()
        {
            this.DisplayedVM = this.mainWorkspaceVM;
        }

        /// <summary>
        /// Navigate to event editor view. 
        /// Begin editing on provided calendar event view model.
        /// </summary>
        /// <param name="calendarEventVM">View model of calendar event to edit.</param>
        public void NavigateToEventEditorViewOnEdit(CalendarEventVM calendarEventVM)
        {
            this.eventEditorVM.BeginEdit(calendarEventVM);
            this.DisplayedVM = this.eventEditorVM;
        }

        /// <summary>
        /// Navigate to event editor view. 
        /// Begin adding calendar event with provided start time.
        /// </summary>
        /// <param name="startTime">View model of calendar event to edit.</param>
        public void NavigateToEventEditorViewOnAdd(DateTime? startTime)
        {
            this.eventEditorVM.BeginAdd(startTime);
            this.DisplayedVM = this.eventEditorVM;
        }
    }
}
