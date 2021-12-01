using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    public enum ReminderTimeOption
    {
        Immediately,
        Minutes5,
        Minutes15,
        Minutes30,
        Hour1,
        Hour12,
        Day1,
        Week1,
        Custom
    }

    public class EventEditorVM : ViewModelBase
    {
        private NavigationVM navigationVM;
        private CalendarEventVM calendarEventVM;
        private CalendarCategoryVMs calendarCategoryVMs;
        private CalendarEventVMs calendarEventVMs;

        private static ReminderTimeOption[] reminderTimeOptions =
        {
            ReminderTimeOption.Immediately,
            ReminderTimeOption.Minutes5,
            ReminderTimeOption.Minutes30,
            ReminderTimeOption.Hour1,
            ReminderTimeOption.Hour12,
            ReminderTimeOption.Day1,
            ReminderTimeOption.Week1,
            ReminderTimeOption.Custom
        };
        private bool useCustomReminderTime;

        private ICommand cancelCommand;
        private ICommand saveCommand;
        private ICommand deleteCommand;
        bool editMode = false;

        private string subject;
        private DateTime startTime;
        private DateTime endTime;
        private bool allDay;
        private ReminderTimeOption chosenReminderTimeOption;
        private TimeSpan customReminderTime;
        private CalendarCategoryVM calendarCategory;
        private string description;

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

        public static ReminderTimeOption[] ReminderTimeOptions => reminderTimeOptions;

        /// <summary>
        /// Display custom reminder time picker if true.
        /// </summary>
        public bool UseCustomReminderTime
        {
            get
            {
                return useCustomReminderTime;
            }
            set
            {
                useCustomReminderTime = value;
                NotifyPropertyChanged();
            }
        }

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
        /// Chosen reminder time option.
        /// </summary>
        public ReminderTimeOption ChosenReminderTimeOption
        {
            get
            {
                return chosenReminderTimeOption;
            }
            set
            {
                chosenReminderTimeOption = value;
                if (value == ReminderTimeOption.Custom)
                    this.UseCustomReminderTime = true;
                else
                    this.UseCustomReminderTime = false;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Custom reminder time.
        /// </summary>
        public TimeSpan CustomReminderTime
        {
            get
            {
                return customReminderTime;
            }
            set
            {
                customReminderTime = value;
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

            this.cancelCommand = new RelayCommand(i => this.HideView(), i => true);
            this.saveCommand = new RelayCommand(i => this.SaveCalendarEvent(), i => true);
            this.deleteCommand = new RelayCommand(i => this.DeleteCalendarEvent(), i => this.editMode);
        }

        /// <summary>
        /// Begin add mode. Add a new event with the provided start time.
        /// </summary>
        /// <param name="startTime">Start time</param>
        public void BeginAdd(DateTime? startTime)
        {
            if (startTime == null)
            {
                this.CalendarEventVM = new CalendarEventVM(this.navigationVM)
                {
                    CalendarCategoryVM = this.CalendarCategories[0],
                    ReminderTime = new TimeSpan(0, 30, 0)
                };
            }
            else
            {
                this.CalendarEventVM = new CalendarEventVM(this.navigationVM, startTime)
                {
                    CalendarCategoryVM = this.CalendarCategories[0],
                    ReminderTime = new TimeSpan(0, 30, 0)
                };
            }
        }

        /// <summary>
        /// Begin edit mode. Edit directly on the provided calendar event view model.
        /// </summary>
        /// <param name="calendarEventVM">View model of calendar event to edit.</param>
        public void BeginEdit(CalendarEventVM calendarEventVM)
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
            SetChosenReminderTimeOption();
            this.CustomReminderTime = this.calendarEventVM.ReminderTime;
            this.CalendarCategory = this.calendarEventVM.CalendarCategoryVM;
            this.Description = this.calendarEventVM.Description;
        }

        /// <summary>
        /// Set chosen reminder time option from
        /// provided calendar event view model's reminder time.
        /// </summary>
        private void SetChosenReminderTimeOption()
        {
            TimeSpan reminderTime = this.calendarEventVM.ReminderTime;
            if (reminderTime == new TimeSpan(0, 0, 0))
                this.ChosenReminderTimeOption = ReminderTimeOption.Immediately;
            else if (reminderTime == new TimeSpan(0, 5, 0))
                this.ChosenReminderTimeOption = ReminderTimeOption.Minutes5;
            else if (reminderTime == new TimeSpan(0, 15, 0))
                this.ChosenReminderTimeOption = ReminderTimeOption.Minutes15;
            else if (reminderTime == new TimeSpan(0, 30, 0))
                this.ChosenReminderTimeOption = ReminderTimeOption.Minutes30;
            else if (reminderTime == new TimeSpan(1, 0, 0))
                this.ChosenReminderTimeOption = ReminderTimeOption.Hour1;
            else if (reminderTime == new TimeSpan(12, 0, 0))
                this.ChosenReminderTimeOption = ReminderTimeOption.Hour12;
            else if (reminderTime == new TimeSpan(1, 0, 0, 0))
                this.ChosenReminderTimeOption = ReminderTimeOption.Day1;
            else if (reminderTime == new TimeSpan(7, 0, 0, 0))
                this.ChosenReminderTimeOption = ReminderTimeOption.Week1;
            else
                this.ChosenReminderTimeOption = ReminderTimeOption.Custom;
        }

        /// <summary>
        /// Save calendar event into database and go back to main workspace view.
        /// </summary>
        private void SaveCalendarEvent()
        {
            this.calendarEventVM.Subject = this.Subject;
            SetStartEndTimeOnEventVM();
            SetReminderTimeOnEventVM();
            this.calendarEventVM.AllDay = this.AllDay;
            this.calendarEventVM.CalendarCategoryVM = this.CalendarCategory;
            this.calendarEventVM.Description = this.Description;

            if (this.editMode)
                this.calendarEventVMs.EditCalendarEvent(this.calendarEventVM);
            else
                this.calendarEventVMs.AddCalendarEvent(this.calendarEventVM);

            this.editMode = false;
            this.navigationVM.NavigateToMainWorkspaceView();
        }

        /// <summary>
        /// Set reminder time on calendar event view model
        /// from chosen reminder time option.
        /// </summary>
        private void SetReminderTimeOnEventVM()
        {
            switch (this.ChosenReminderTimeOption)
            {
                case ReminderTimeOption.Immediately:
                    this.calendarEventVM.ReminderTime = new TimeSpan(0, 0, 0);
                    break;
                case ReminderTimeOption.Minutes15:
                    this.calendarEventVM.ReminderTime = new TimeSpan(0, 15, 0);
                    break;
                case ReminderTimeOption.Minutes30:
                    this.calendarEventVM.ReminderTime = new TimeSpan(0, 30, 0);
                    break;
                case ReminderTimeOption.Hour1:
                    this.calendarEventVM.ReminderTime = new TimeSpan(1, 0, 0);
                    break;
                case ReminderTimeOption.Hour12:
                    this.calendarEventVM.ReminderTime = new TimeSpan(12, 0, 0);
                    break;
                case ReminderTimeOption.Day1:
                    this.calendarEventVM.ReminderTime = new TimeSpan(1, 0, 0, 0);
                    break;
                case ReminderTimeOption.Week1:
                    this.calendarEventVM.ReminderTime = new TimeSpan(7, 0, 0, 0);
                    break;
                default:
                    this.calendarEventVM.ReminderTime = this.CustomReminderTime;
                    break;
            }
        }

        /// <summary>
        /// Set start and end time on calendar event view model.
        /// Ignore hour and minute selection if All Day checkbox is checked.
        /// </summary>
        private void SetStartEndTimeOnEventVM()
        {
            if (this.AllDay)
            {
                this.calendarEventVM.StartTime = this.StartTime.Date;
                this.calendarEventVM.EndTime = this.EndTime.Date;
            }
            else
            {
                this.calendarEventVM.StartTime = this.StartTime;
                this.calendarEventVM.EndTime = this.EndTime;
            }
        }

        /// <summary>
        /// Delete calendar event from database and go back to main workspace view.
        /// </summary>
        private void DeleteCalendarEvent()
        {
            this.calendarEventVMs.DeleteCalendarEvent(this.calendarEventVM);
            this.editMode = false;
            this.navigationVM.NavigateToMainWorkspaceView();
        }

        private void HideView()
        {
            this.editMode = false;
            this.navigationVM.NavigateToMainWorkspaceView();
        }
    }
}
