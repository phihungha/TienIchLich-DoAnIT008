using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    public class EventEditorViewModel : ViewModelBase
    {
        private MasterViewModel masterVM;
        private CalendarEventViewModel calendarEventVM;
        private ObservableCollection<CalendarCategoryViewModel> calendarCategories;
        private ICommand cancelCommand;
        private ICommand saveCommand;
        private ICommand deleteCommand;
        bool editMode = false;

        string subject;
        DateTime startTime;
        DateTime endTime;
        bool allDay;
        TimeSpan reminderTime;
        CalendarCategoryViewModel calendarCategory;
        string description = "";

        public CalendarEventViewModel CalendarEventVM
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

        public ObservableCollection<CalendarCategoryViewModel> CalendarCategories
        {
            get
            {
                return calendarCategories;
            }
            set
            {
                calendarCategories = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return cancelCommand;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return saveCommand;
            }
        }

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
        public CalendarCategoryViewModel CalendarCategory
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

        public EventEditorViewModel(MasterViewModel masterVM)
        {
            this.masterVM = masterVM;
            this.calendarCategories = masterVM.CalendarData.CalendarCategories;
            this.cancelCommand = new RelayCommand(i => masterVM.NavigateToMainView(), i => true);
            this.saveCommand = new RelayCommand(i => this.SaveCalendarEvent(), i => true);
            this.deleteCommand = new RelayCommand(i => this.SaveCalendarEvent(), i => this.editMode);

            if (!this.EditMode)
                this.CalendarEventVM = new CalendarEventViewModel();
        }

        private void SetCalendarEvent()
        {
            this.Subject = this.calendarEventVM.Subject;
            this.StartTime = this.calendarEventVM.StartTime;
            this.EndTime = this.calendarEventVM.EndTime;
            this.AllDay = this.calendarEventVM.AllDay;
            this.ReminderTime = this.calendarEventVM.ReminderTime;
            this.CalendarCategory = this.calendarEventVM.CalendarCategory;
            this.Description = this.calendarEventVM.Description;
        }

        private void SaveCalendarEvent()
        {
            this.calendarEventVM.Subject = this.Subject;
            this.calendarEventVM.StartTime = this.StartTime;
            this.calendarEventVM.EndTime = this.EndTime;
            this.calendarEventVM.AllDay = this.AllDay;
            this.calendarEventVM.ReminderTime = this.ReminderTime;
            this.calendarEventVM.CalendarCategory = this.CalendarCategory;
            this.calendarEventVM.Description = this.Description;

            if (this.editMode)
                this.masterVM.CalendarData.EditCalendarEvent(this.calendarEventVM);
            else
                this.masterVM.CalendarData.AddCalendarEvent(this.calendarEventVM);

            this.masterVM.NavigateToMainView();
        }

        private void DeleteCalendarEvent()
        {
            this.masterVM.CalendarData.DeleteCalendarEvent(this.calendarEventVM);
        }
    }
}
