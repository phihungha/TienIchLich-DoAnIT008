using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    public class EventEditorVM : ViewModelBase
    {
        private NavigationVM navigationVM;
        private CalendarEventVM calendarEventVM;
        private CalendarCategoryVMs calendarCategoryVMs;
        private CalendarEventVMs calendarEventVMs;

        private ICommand cancelCommand;
        private ICommand saveCommand;
        private ICommand deleteCommand;
        bool editMode = false;

        string subject;
        DateTime startTime;
        DateTime endTime;
        bool allDay;
        TimeSpan reminderTime;
        CalendarCategoryVM calendarCategory;
        string description = "";

        /// <summary>
        /// View model of currently edited calendar event.
        /// </summary>
        public CalendarEventVM CalendarEventVM
        {
            get
            {
                return calendarEventVM;
            }
            set
            {
                calendarEventVM = value;
                this.SetCalendarEvent();
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// View models of calendar categories to select.
        /// </summary>
        public ObservableCollection<CalendarCategoryVM> CalendarCategories
        { get => calendarCategoryVMs.CategoryVMs; }

        /// <summary>
        /// Discard changes. Go to main workspace view.
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                return cancelCommand;
            }
        }

        /// <summary>
        /// Save event and go back to main workspace view.
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                return saveCommand;
            }
        }

        /// <summary>
        /// Delete event and go back to main workspace view.
        /// </summary>
        public ICommand DeleteCommand
        {
            get
            {
                return deleteCommand;
            }
        }

        /// <summary>
        /// Is the editor in edit mode.
        /// In edit mode, directly edit the event VM.
        /// Otherwise, create a new event VM.
        /// </summary>
        public bool EditMode
        {
            get
            {
                return editMode;
            }
            set
            {
                editMode = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Subject of event.
        /// </summary>
        public string Subject
        {
            get
            {
                return subject;
            }
            set
            {
                subject = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Event start time.
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Event end time.
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                endTime = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Event happens in an entire day. 
        /// </summary>
        public bool AllDay
        {
            get
            {
                return allDay;
            }
            set
            {
                allDay = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Time to remind users before event starts.
        /// </summary>
        public TimeSpan ReminderTime
        {
            get
            {
                return reminderTime;
            }
            set
            {
                reminderTime = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Description of event.
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Calendar category of event.
        /// </summary>
        public CalendarCategoryVM CalendarCategory
        {
            get
            {
                return calendarCategory;
            }
            set
            {
                calendarCategory = value;
                NotifyPropertyChanged();
            }
        }

        public EventEditorVM(NavigationVM navigationVM, CalendarEventVMs calendarEventVMs, CalendarCategoryVMs calendarCategoryVMs)
        {
            this.navigationVM = navigationVM;
            this.calendarCategoryVMs = calendarCategoryVMs;
            this.calendarEventVMs = calendarEventVMs;
            this.cancelCommand = new RelayCommand(i => navigationVM.NavigateToMainWorkspaceView(), i => true);
            this.saveCommand = new RelayCommand(i => this.SaveCalendarEvent(), i => true);
            this.deleteCommand = new RelayCommand(i => this.DeleteCalendarEvent(), i => this.editMode);

            // View model for a new event.
            if (!this.EditMode)
                this.CalendarEventVM = new CalendarEventVM(this.navigationVM);
        }

        /// <summary>
        /// Enable edit mode. Edit directly on the provided calendar event view model.
        /// </summary>
        /// <param name="calendarEventVM">View model of calendar event to edit.</param>
        public void EnableEditMode(CalendarEventVM calendarEventVM)
        {
            this.editMode = true;
            this.CalendarEventVM = calendarEventVM;
        }

        /// <summary>
        /// Set all displayed properties to the provided calendar event view model.
        /// </summary>
        private void SetCalendarEvent()
        {
            this.Subject = this.calendarEventVM.Subject;
            this.StartTime = this.calendarEventVM.StartTime;
            this.EndTime = this.calendarEventVM.EndTime;
            this.AllDay = this.calendarEventVM.AllDay;
            this.ReminderTime = this.calendarEventVM.ReminderTime;
            this.CalendarCategory = this.calendarEventVM.CalendarCategoryVM;
            this.Description = this.calendarEventVM.Description;
        }

        /// <summary>
        /// Save calendar event into database and go back to main workspace view.
        /// </summary>
        private void SaveCalendarEvent()
        {
            this.calendarEventVM.Subject = this.Subject;
            this.calendarEventVM.StartTime = this.StartTime;
            this.calendarEventVM.EndTime = this.EndTime;
            this.calendarEventVM.AllDay = this.AllDay;
            this.calendarEventVM.ReminderTime = this.ReminderTime;
            this.calendarEventVM.CalendarCategoryVM = this.CalendarCategory;
            this.calendarEventVM.Description = this.Description;

            if (this.editMode)
                this.calendarEventVMs.EditCalendarEvent(this.calendarEventVM);
            else
                this.calendarEventVMs.AddCalendarEvent(this.calendarEventVM);

            this.navigationVM.NavigateToMainWorkspaceView();
        }

        /// <summary>
        /// Delete calendar event from database and go back to main workspace view.
        /// </summary>
        private void DeleteCalendarEvent()
        {
            this.calendarEventVMs.DeleteCalendarEvent(this.calendarEventVM);
            this.navigationVM.NavigateToMainWorkspaceView();
        }
    }
}
