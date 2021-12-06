using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    public enum ReminderTimeOptionId
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

    public struct ReminderTimeOption
    {
        public ReminderTimeOptionId Id { get; set; }
        public TimeSpan Timespan { get; set; }
    }

    public class EventEditorVM : ViewModelBase
    {
        private NavigationVM navigationVM;
        private CalendarEventVM calendarEventVM;
        private ObservableCollection<CalendarCategoryVM> calendarCategoryVMs;
        private CalendarEventVMManager calendarEventVMManager;

        private static ReminderTimeOption[] reminderTimeOptions =
        {
            new ReminderTimeOption() { Id = ReminderTimeOptionId.Immediately, Timespan = new TimeSpan(0) },
            new ReminderTimeOption() { Id = ReminderTimeOptionId.Minutes5, Timespan = new TimeSpan(0, 5, 0) },
            new ReminderTimeOption() { Id = ReminderTimeOptionId.Minutes30, Timespan = new TimeSpan(0, 30, 0) },
            new ReminderTimeOption() { Id = ReminderTimeOptionId.Hour1, Timespan = new TimeSpan(1, 0, 0) },
            new ReminderTimeOption() { Id = ReminderTimeOptionId.Hour12, Timespan = new TimeSpan(12, 0, 0) },
            new ReminderTimeOption() { Id = ReminderTimeOptionId.Day1, Timespan = new TimeSpan(1, 0, 0, 0) },
            new ReminderTimeOption() { Id = ReminderTimeOptionId.Week1, Timespan = new TimeSpan(7, 0, 0, 0) },
            new ReminderTimeOption() { Id = ReminderTimeOptionId.Custom, Timespan = new TimeSpan() }
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
        private ReminderTimeOption selectedReminderTimeOption;
        private TimeSpan customReminderTimeOption;
        private CalendarCategoryVM calendarCategoryVM;
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
                this.SetEditorFieldsFromEventVM();
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// View models of calendar categories to select.
        /// </summary>
        public ObservableCollection<CalendarCategoryVM> CalendarCategoryVMs
        { get => calendarCategoryVMs; }

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
        public ReminderTimeOption SelectedReminderTimeOption
        {
            get
            {
                return selectedReminderTimeOption;
            }
            set
            {
                selectedReminderTimeOption = value;
                if (value.Id == ReminderTimeOptionId.Custom)
                    this.UseCustomReminderTime = true;
                else
                    this.UseCustomReminderTime = false;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Custom reminder time.
        /// </summary>
        public TimeSpan CustomReminderTimeOption
        {
            get
            {
                return customReminderTimeOption;
            }
            set
            {
                customReminderTimeOption = value;
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
        public CalendarCategoryVM CalendarCategoryVM
        {
            get
            {
                return calendarCategoryVM;
            }
            set
            {
                calendarCategoryVM = value;
                NotifyPropertyChanged();
            }
        }

        public EventEditorVM(NavigationVM navigationVM, CalendarEventVMManager calendarEventVMs, ObservableCollection<CalendarCategoryVM> calendarCategoryVMs)
        {
            this.navigationVM = navigationVM;
            this.calendarCategoryVMs = calendarCategoryVMs;
            this.calendarEventVMManager = calendarEventVMs;

            this.cancelCommand = new RelayCommand(i => this.HideView(), i => true);
            this.saveCommand = new RelayCommand(i => this.SaveEvent(), i => true);
            this.deleteCommand = new RelayCommand(i => this.DeleteEvent(), i => this.editMode);
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
                    CalendarCategoryVM = this.CalendarCategoryVMs[0],
                    ReminderTime = new TimeSpan(0, 30, 0)
                };
            }
            else
            {
                this.CalendarEventVM = new CalendarEventVM(this.navigationVM, startTime)
                {
                    CalendarCategoryVM = this.CalendarCategoryVMs[0],
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
        private void SetEditorFieldsFromEventVM()
        {
            this.Subject = this.calendarEventVM.Subject;
            this.StartTime = this.calendarEventVM.StartTime;
            this.EndTime = this.calendarEventVM.EndTime;
            this.AllDay = this.calendarEventVM.AllDay;
            SetSelectedReminderTimeOptionFromEventVM();
            this.CustomReminderTimeOption = this.calendarEventVM.ReminderTime;
            this.CalendarCategoryVM = this.calendarEventVM.CalendarCategoryVM;
            this.Description = this.calendarEventVM.Description;
        }

        /// <summary>
        /// Set chosen reminder time option from
        /// provided calendar event view model's reminder time.
        /// </summary>
        private void SetSelectedReminderTimeOptionFromEventVM()
        {
            this.SelectedReminderTimeOption = ReminderTimeOptions
                .Where(i => i.Timespan == this.calendarEventVM.ReminderTime)
                .DefaultIfEmpty(ReminderTimeOptions[7])
                .First();
        }

        /// <summary>
        /// Save calendar event into database and go back to main workspace view.
        /// </summary>
        private void SaveEvent()
        {
            this.calendarEventVM.Subject = this.Subject;
            this.calendarEventVM.StartTime = this.StartTime;
            this.calendarEventVM.EndTime = this.EndTime;
            SetReminderTimeOnEventVM();
            this.calendarEventVM.AllDay = this.AllDay;
            this.calendarEventVM.CalendarCategoryVM = this.CalendarCategoryVM;
            this.calendarEventVM.Description = this.Description;

            if (this.editMode)
                this.calendarEventVMManager.EditCalendarEvent(this.calendarEventVM);
            else
                this.calendarEventVMManager.AddCalendarEvent(this.calendarEventVM);

            this.editMode = false;
            this.navigationVM.NavigateToMainWorkspaceView();
        }

        /// <summary>
        /// Set reminder time on calendar event view model
        /// from chosen reminder time option.
        /// </summary>
        private void SetReminderTimeOnEventVM()
        {
            if (this.SelectedReminderTimeOption.Id == ReminderTimeOptionId.Custom)
                this.calendarEventVM.ReminderTime = this.CustomReminderTimeOption;
            else
                this.calendarEventVM.ReminderTime = this.SelectedReminderTimeOption.Timespan;
        }

        /// <summary>
        /// Delete calendar event from database and go back to main workspace view.
        /// </summary>
        private void DeleteEvent()
        {
            this.calendarEventVMManager.DeleteCalendarEvent(this.calendarEventVM);
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
