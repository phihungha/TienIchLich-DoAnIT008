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

        private ReminderVM reminderVM;

        /// <summary>
        /// Reminder view.
        /// </summary>
        public ReminderVM ReminderVM
        {
            set
            {
                reminderVM = value;
                NotifyPropertyChanged();
            }
        }

        private SettingsVM settingsVM;

        /// <summary>
        /// Settings view.
        /// </summary>
        public SettingsVM SettingsVM
        {
            set
            {
                settingsVM = value;
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
        public void NavigateToEventEditorViewToEdit(CalendarEventVM eventVM)
        {
            eventEditorVM.BeginEdit(eventVM);
            DisplayedVM = eventEditorVM;
        }

        /// <summary>
        /// Navigate to event editor view to add a new calendar event.
        /// </summary>
        /// <param name="startTime">Start time of new calendar event</param>
        public void NavigateToEventEditorViewToAdd(DateTime? startTime)
        {
            if (eventEditorVM.BeginAdd(startTime))
                DisplayedVM = eventEditorVM;
        }

        /// <summary>
        /// Navigate to reminder view.
        /// </summary>
        /// <param name="eventVM">Calendar event view model to display</param>
        public void NavigateToReminderView(CalendarEventVM eventVM)
        {
            reminderVM.Remind(eventVM);
            DisplayedVM = reminderVM;
        }

        /// <summary>
        /// Navigate to settings view.
        /// </summary>
        public void NavigateToSettingsView()
        {
            DisplayedVM = settingsVM;
        }
    }
}