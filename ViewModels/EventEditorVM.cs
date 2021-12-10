using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TienIchLich.Services;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// Wrapper class for calendar event start and end time.
    /// Used to pass time as reference.
    /// </summary>
    public class CalendarEventTime
    {
        /// <summary>
        /// Event start time.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Event end time.
        /// </summary>
        public DateTime EndTime { get; set; }
    }

    /// <summary>
    /// View model for calendar event time picker view.
    /// </summary>
    public class CalendarEventTimePickerVM : ViewModelBase
    {
        private CalendarEventTime eventTime;

        public DateTime StartTime
        {
            get
            {
                return eventTime.StartTime;
            }
            set
            {
                eventTime.StartTime = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime EndTime
        {
            get
            {
                return eventTime.EndTime;
            }
            set
            {
                eventTime.EndTime = value;
                NotifyPropertyChanged();
            }
        }

        public CalendarEventTimePickerVM(CalendarEventTime eventTime)
        {
            this.eventTime = eventTime;
        }
    }

    /// <summary>
    /// View model for calendar event datetime picker view.
    /// </summary>
    public class CalendarEventDatetimePickerVM : CalendarEventTimePickerVM
    {
        public CalendarEventDatetimePickerVM(CalendarEventTime eventTime)
            : base(eventTime)
        {

        }
    }

    /// <summary>
    /// View model for calendar event date picker view.
    /// </summary>
    public class CalendarEventDatePickerVM : CalendarEventTimePickerVM
    {
        public CalendarEventDatePickerVM(CalendarEventTime eventTime)
            : base(eventTime)
        {

        }
    }

    public enum EventReminderTimeOptionId
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

    public struct EventReminderTimeOption
    {
        public EventReminderTimeOptionId Id { get; set; }
        public TimeSpan Timespan { get; set; }
    }

    public class EventEditorVM : ViewModelBase, IDataErrorInfo
    {
        private NavigationVM navigationVM;
        private CalendarEventVM calendarEventVM;
        private ObservableCollection<CalendarCategoryVM> categoryVMs;
        private CalendarEventVMManager eventVMManager;
        private DialogService dialogService;

        private static EventReminderTimeOption[] reminderTimeOptions =
        {
            new EventReminderTimeOption() { Id = EventReminderTimeOptionId.Immediately, Timespan = new TimeSpan(0) },
            new EventReminderTimeOption() { Id = EventReminderTimeOptionId.Minutes5, Timespan = new TimeSpan(0, 5, 0) },
            new EventReminderTimeOption() { Id = EventReminderTimeOptionId.Minutes30, Timespan = new TimeSpan(0, 30, 0) },
            new EventReminderTimeOption() { Id = EventReminderTimeOptionId.Hour1, Timespan = new TimeSpan(1, 0, 0) },
            new EventReminderTimeOption() { Id = EventReminderTimeOptionId.Hour12, Timespan = new TimeSpan(12, 0, 0) },
            new EventReminderTimeOption() { Id = EventReminderTimeOptionId.Day1, Timespan = new TimeSpan(1, 0, 0, 0) },
            new EventReminderTimeOption() { Id = EventReminderTimeOptionId.Week1, Timespan = new TimeSpan(7, 0, 0, 0) },
            new EventReminderTimeOption() { Id = EventReminderTimeOptionId.Custom, Timespan = new TimeSpan() }
        };
        private bool useCustomReminderTime;

        private ICommand cancelCommand;
        private ICommand saveCommand;
        private ICommand deleteCommand;
        private bool editMode = false; // In edit mode, the provided calendar event view model is edited/deleted.
        private bool canSave = true; // Can what is in the editor be saved as a calendar event.

        private CalendarEventTime eventTime = new();
        private CalendarEventDatetimePickerVM eventDatetimePickerVM;
        private CalendarEventDatePickerVM eventDatePickerVM;
        private CalendarEventTimePickerVM eventTimePickerVM;

        private string subject;
        private bool allDay;
        private EventReminderTimeOption selectedReminderTimeOption;
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
        { get => categoryVMs; }

        public static EventReminderTimeOption[] ReminderTimeOptions => reminderTimeOptions;

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
        /// If edit mode is true, directly edit the provided event VM.
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
        /// Decides what time picker will display (DatetimePicker or DatePicker).
        /// </summary>
        public CalendarEventTimePickerVM EventTimePickerVM
        {
            get
            {
                return this.eventTimePickerVM;
            }
            set
            {
                this.eventTimePickerVM = value;
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
                this.SetTimePickerVM();
                NotifyPropertyChanged();
            }
        }


        /// <summary>
        /// Chosen reminder time option.
        /// </summary>
        public EventReminderTimeOption SelectedReminderTimeOption
        {
            get
            {
                return selectedReminderTimeOption;
            }
            set
            {
                selectedReminderTimeOption = value;
                if (value.Id == EventReminderTimeOptionId.Custom)
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

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "Subject")
                {
                    if (this.Subject == "")
                    {
                        result = "Chủ đề không được rỗng!";
                        this.canSave = false;
                    }
                    else
                        this.canSave = true;
                }
                return result;
            }
        }

        public EventEditorVM(NavigationVM navigationVM, DialogService dialogService, CalendarEventVMManager calendarEventVMs, ObservableCollection<CalendarCategoryVM> calendarCategoryVMs)
        {
            this.navigationVM = navigationVM;
            this.dialogService = dialogService;
            this.categoryVMs = calendarCategoryVMs;
            this.eventVMManager = calendarEventVMs;

            this.cancelCommand = new RelayCommand(i => this.HideView(), i => true);
            this.saveCommand = new RelayCommand(i => this.SaveEvent(), i => this.canSave);
            this.deleteCommand = new RelayCommand(i => this.DeleteEvent(), i => this.editMode);

            this.eventDatePickerVM = new CalendarEventDatePickerVM(this.eventTime);
            this.eventDatetimePickerVM = new CalendarEventDatetimePickerVM(this.eventTime);
        }

        private void SetTimePickerVM()
        {
            if (allDay)
                this.EventTimePickerVM = this.eventDatePickerVM;
            else
                this.EventTimePickerVM = this.eventDatetimePickerVM;
        }

        /// <summary>
        /// Begin add mode. Add a new event with the provided start time.
        /// </summary>
        /// <param name="startTime">Start time</param>
        public void BeginAdd(DateTime? startTime)
        {
            if (startTime == null)
            {
                this.CalendarEventVM = new CalendarEventVM(this.navigationVM, this.eventVMManager)
                {
                    CalendarCategoryVM = this.CalendarCategoryVMs[0],
                    ReminderTime = new TimeSpan(0, 30, 0)
                };
            }
            else
            {
                this.CalendarEventVM = new CalendarEventVM(this.navigationVM, this.eventVMManager, startTime)
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
            this.AllDay = this.calendarEventVM.AllDay;
            this.eventTime.StartTime = this.calendarEventVM.StartTime;
            this.eventTime.EndTime = this.calendarEventVM.EndTime;
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
            this.calendarEventVM.StartTime = this.eventTime.StartTime;
            this.calendarEventVM.EndTime = this.eventTime.EndTime;
            SetStartEndTimeOnEventVM();
            SetReminderTimeOnEventVM();
            this.calendarEventVM.AllDay = this.AllDay;
            this.calendarEventVM.CalendarCategoryVM = this.CalendarCategoryVM;
            this.calendarEventVM.Description = this.Description;

            if (this.editMode)
                this.eventVMManager.EditCalendarEvent(this.calendarEventVM);
            else
                this.eventVMManager.AddCalendarEvent(this.calendarEventVM);

            this.editMode = false;
            this.navigationVM.NavigateToMainWorkspaceView();
        }

        /// <summary>
        /// Set start and end time on calendar event view model.
        /// Ignore hour and minute selection if All Day checkbox is checked.
        /// </summary>
        private void SetStartEndTimeOnEventVM()
        {
            if (this.AllDay)
            {
                this.calendarEventVM.StartTime = this.eventTime.StartTime.Date;
                this.calendarEventVM.EndTime = this.eventTime.EndTime.Date;
            }
            else
            {
                this.calendarEventVM.StartTime = this.eventTime.StartTime;
                this.calendarEventVM.EndTime = this.eventTime.EndTime;
            }
        }

        /// <summary>
        /// Set reminder time on calendar event view model
        /// from chosen reminder time option.
        /// </summary>
        private void SetReminderTimeOnEventVM()
        {
            if (this.SelectedReminderTimeOption.Id == EventReminderTimeOptionId.Custom)
                this.calendarEventVM.ReminderTime = this.CustomReminderTimeOption;
            else
                this.calendarEventVM.ReminderTime = this.SelectedReminderTimeOption.Timespan;
        }

        /// <summary>
        /// Delete calendar event from database and go back to main workspace view.
        /// </summary>
        private void DeleteEvent()
        {
            if (this.dialogService.ShowConfirmation("Bạn có muốn xóa sự kiện này?"))
            {
                this.eventVMManager.DeleteCalendarEvent(this.calendarEventVM);
                this.editMode = false;
                this.navigationVM.NavigateToMainWorkspaceView();
            }
        }

        private void HideView()
        {
            this.editMode = false;
            this.navigationVM.NavigateToMainWorkspaceView();
        }
    }
}
