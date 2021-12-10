using System;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model to use in navigating between main workspace and event editor.
    /// </summary>
    public class NavigationVM : ViewModelBase
    {
        private ViewModelBase displayedVM;

        /// <summary>
        /// Currently displayed view.
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

        private MainWorkspaceVM mainWorkspaceVM;

        /// <summary>
        /// Main workspace view
        /// </summary>
        public MainWorkspaceVM MainWorkspaceVM
        {
            set
            {
                mainWorkspaceVM = value;
                NotifyPropertyChanged();
            }
        }

        private EventEditorVM eventEditorVM;

        /// <summary>
        /// Event editor view.
        /// </summary>
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
            DisplayedVM = mainWorkspaceVM;
        }

        /// <summary>
        /// Navigate to event editor view to edit a calendar event.
        /// </summary>
        /// <param name="calendarEventVM">View model of calendar event to edit</param>
        public void NavigateToEventEditorViewToEdit(CalendarEventVM calendarEventVM)
        {
            eventEditorVM.BeginEdit(calendarEventVM);
            DisplayedVM = eventEditorVM;
        }

        /// <summary>
        /// Navigate to event editor view to add a new calendar event.
        /// </summary>
        /// <param name="startTime">Start time of new calendar event</param>
        public void NavigateToEventEditorViewToAdd(DateTime? startTime)
        {
            eventEditorVM.BeginAdd(startTime);
            DisplayedVM = eventEditorVM;
        }
    }
}