using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TienIchLich.Services;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// Identifiers for calendar event reminder time options.
    /// </summary>
    public enum CalendarEventReminderTimeOptionId
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

    /// <summary>
    /// Info of a calendar event reminder time option.
    /// </summary>
    public struct CalendarEventReminderTimeOption
    {
        public CalendarEventReminderTimeOptionId Id { get; set; }
        public TimeSpan Time { get; set; }
    }

    /// <summary>
    /// View model for the event editor view.
    /// </summary>
    public class EventEditorVM : ViewModelBase, IDataErrorInfo
    {
        private NavigationVM navigationVM;
        private DialogService dialogService;
        private CalendarEventVMManager eventVMManager;

        private CalendarEventVM calendarEventVM;

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
                SetEditorFieldsFromEventVM();
                NotifyPropertyChanged();
            }
        }

        private DateTime startTime;

        /// <summary>
        /// Event's start time.
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

        private DateTime endTime;

        /// <summary>
        /// Event's end time.
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


        private string subject;

        /// <summary>
        /// Subject of this event.
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

        private bool allDay;

        /// <summary>
        /// True of this event happens in an entire day.
        /// Use datetime picker if False. Use date-only picker if true.
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
                DisplayTimePicker = !value;
                NotifyPropertyChanged();
            }
        }

        private bool displayTimePicker = false;

        /// <summary>
        /// True of time picker should display.
        /// </summary>
        public bool DisplayTimePicker
        {
            get
            {
                return displayTimePicker;
            }
            set
            {
                displayTimePicker = value;
                NotifyPropertyChanged();
            }
        }

        private static CalendarEventReminderTimeOption[] reminderTimeOptions =
        {
            new CalendarEventReminderTimeOption() { Id = CalendarEventReminderTimeOptionId.Immediately, Time = new TimeSpan(0) },
            new CalendarEventReminderTimeOption() { Id = CalendarEventReminderTimeOptionId.Minutes5, Time = new TimeSpan(0, 5, 0) },
            new CalendarEventReminderTimeOption() { Id = CalendarEventReminderTimeOptionId.Minutes30, Time = new TimeSpan(0, 30, 0) },
            new CalendarEventReminderTimeOption() { Id = CalendarEventReminderTimeOptionId.Hour1, Time = new TimeSpan(1, 0, 0) },
            new CalendarEventReminderTimeOption() { Id = CalendarEventReminderTimeOptionId.Hour12, Time = new TimeSpan(12, 0, 0) },
            new CalendarEventReminderTimeOption() { Id = CalendarEventReminderTimeOptionId.Day1, Time = new TimeSpan(1, 0, 0, 0) },
            new CalendarEventReminderTimeOption() { Id = CalendarEventReminderTimeOptionId.Week1, Time = new TimeSpan(7, 0, 0, 0) },
            new CalendarEventReminderTimeOption() { Id = CalendarEventReminderTimeOptionId.Custom, Time = new TimeSpan() }
        };

        /// <summary>
        /// Reminder time options.
        /// </summary>
        public static CalendarEventReminderTimeOption[] ReminderTimeOptions => reminderTimeOptions;

        private bool useCustomReminderTime;

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

        private CalendarEventReminderTimeOption selectedReminderTimeOption;

        /// <summary>
        /// Selected reminder time option.
        /// </summary>
        public CalendarEventReminderTimeOption SelectedReminderTimeOption
        {
            get
            {
                return selectedReminderTimeOption;
            }
            set
            {
                selectedReminderTimeOption = value;
                if (value.Id == CalendarEventReminderTimeOptionId.Custom)
                    UseCustomReminderTime = true;
                else
                    UseCustomReminderTime = false;
                NotifyPropertyChanged();
            }
        }

        private TimeSpan customReminderTimeOption;

        /// <summary>
        /// Selected custom reminder time option.
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
        /// View models of calendar categories to select.
        /// </summary>
        public ObservableCollection<CalendarCategoryVM> CategoryVMs { get; private set; }

        private CalendarCategoryVM categoryVM;

        /// <summary>
        /// Calendar category of this event.
        /// </summary>
        public CalendarCategoryVM CategoryVM
        {
            get
            {
                return categoryVM;
            }
            set
            {
                categoryVM = value;
                NotifyPropertyChanged();
            }
        }

        private string description;

        /// <summary>
        /// Description of this event.
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

        // Data validation
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string result = null;

                if (columnName == "Subject")
                {
                    if (Subject == "")
                    {
                        result = "Chủ đề không được rỗng!";
                        canSave = false;
                    }
                }
                else if (columnName == "StartTime" || columnName == "EndTime")
                {
                    if (StartTime > EndTime)
                    {
                        result = "Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc";
                        canSave = false;
                    }
                }

                if (result == null)
                    canSave = true;
                return result;
            }
        }

        // True if this event can be saved.
        private bool canSave = true;

        /// <summary>
        /// Discard changes and go back to main workspace view.
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        /// <summary>
        /// Save event and go back to main workspace view.
        /// </summary>
        public ICommand SaveCommand { get; private set; }

        /// <summary>
        /// Delete event and go back to main workspace view.
        /// </summary>
        public ICommand DeleteCommand { get; private set; }

        // If edit mode is true, edit the provided event view model. Otherwise, create a new event view model.
        private bool editMode = false;

        public EventEditorVM(NavigationVM navigationVM, DialogService dialogService, CalendarEventVMManager calendarEventVMs, ObservableCollection<CalendarCategoryVM> calendarCategoryVMs)
        {
            this.navigationVM = navigationVM;
            this.dialogService = dialogService;
            CategoryVMs = calendarCategoryVMs;
            eventVMManager = calendarEventVMs;

            CancelCommand = new RelayCommand(i => HideView());
            SaveCommand = new RelayCommand(i => SaveEvent(), i => canSave);
            DeleteCommand = new RelayCommand(i => DeleteEvent(), i => editMode);
        }

        /// <summary>
        /// Check if event addition is permitted.
        /// Permitted when there is at least one calendar category.
        /// </summary>
        /// <returns></returns>
        private bool IsAddPermitted()
        {
            if (CategoryVMs.Count() == 0)
            {
                dialogService.ShowError("Bạn cần ít nhất một loại lịch để có thể tạo được sự kiện mới.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Enter add mode. Add a new calendar event with the provided start time.
        /// </summary>
        /// <param name="startTime">Start time</param>
        public bool BeginAdd(DateTime? startTime)
        {
            if (!IsAddPermitted())
                return false;

            if (startTime == null)
            {
                CalendarEventVM = new CalendarEventVM(eventVMManager, navigationVM)
                {
                    CategoryVM = CategoryVMs[0],
                    ReminderTime = new TimeSpan(0, 30, 0)
                };
            }
            else
            {
                CalendarEventVM = new CalendarEventVM(eventVMManager, navigationVM, startTime)
                {
                    CategoryVM = CategoryVMs[0],
                    ReminderTime = new TimeSpan(0, 30, 0)
                };
            }
            return true;
        }

        /// <summary>
        /// Enter edit mode. Edit the provided calendar event view model.
        /// </summary>
        /// <param name="calendarEventVM">View model of calendar event to edit</param>
        public void BeginEdit(CalendarEventVM calendarEventVM)
        {
            editMode = true;
            CalendarEventVM = calendarEventVM;
        }

        /// <summary>
        /// Set all displayed properties to the provided calendar event view model's values.
        /// </summary>
        private void SetEditorFieldsFromEventVM()
        {
            Subject = calendarEventVM.Subject;
            AllDay = calendarEventVM.AllDay;
            StartTime = calendarEventVM.StartTime;
            EndTime = calendarEventVM.EndTime;
            SetSelectedReminderTimeOptionFromEventVM();
            CustomReminderTimeOption = calendarEventVM.ReminderTime;
            CategoryVM = calendarEventVM.CategoryVM;
            Description = calendarEventVM.Description;
        }

        /// <summary>
        /// Set selected reminder time option from
        /// calendar event view model's reminder time value.
        /// </summary>
        private void SetSelectedReminderTimeOptionFromEventVM()
        {
            SelectedReminderTimeOption = ReminderTimeOptions
                .Where(i => i.Time == calendarEventVM.ReminderTime)
                .DefaultIfEmpty(ReminderTimeOptions[7])
                .First();
        }

        /// <summary>
        /// Save calendar event into database and go back to main workspace view.
        /// </summary>
        private void SaveEvent()
        {
            calendarEventVM.Subject = Subject;
            calendarEventVM.StartTime = StartTime;
            calendarEventVM.EndTime = EndTime;
            SetStartEndTimeOnEventVM();
            SetReminderTimeOnEventVM();
            calendarEventVM.AllDay = AllDay;
            calendarEventVM.CategoryVM = CategoryVM;
            calendarEventVM.Description = Description;

            if (editMode)
                eventVMManager.Edit(calendarEventVM);
            else
                eventVMManager.Add(calendarEventVM);

            editMode = false;
            navigationVM.NavigateToMainWorkspaceView();
        }

        /// <summary>
        /// Set start and end time on calendar event view model.
        /// Ignore hour and minute selection if All Day checkbox is checked.
        /// </summary>
        private void SetStartEndTimeOnEventVM()
        {
            if (AllDay)
            {
                calendarEventVM.StartTime = StartTime.Date;
                calendarEventVM.EndTime = EndTime.Date;
            }
            else
            {
                calendarEventVM.StartTime = StartTime;
                calendarEventVM.EndTime = EndTime;
            }
        }

        /// <summary>
        /// Set reminder time on calendar event view model from selected reminder time option.
        /// </summary>
        private void SetReminderTimeOnEventVM()
        {
            if (SelectedReminderTimeOption.Id == CalendarEventReminderTimeOptionId.Custom)
                calendarEventVM.ReminderTime = CustomReminderTimeOption;
            else
                calendarEventVM.ReminderTime = SelectedReminderTimeOption.Time;
        }

        /// <summary>
        /// Delete calendar event from database and go back to main workspace view.
        /// </summary>
        private void DeleteEvent()
        {
            if (dialogService.ShowConfirmation("Bạn có muốn xóa sự kiện này?"))
            {
                eventVMManager.Delete(calendarEventVM);
                editMode = false;
                navigationVM.NavigateToMainWorkspaceView();
            }
        }

        /// <summary>
        /// Return to main workspace view.
        /// </summary>
        private void HideView()
        {
            editMode = false;
            navigationVM.NavigateToMainWorkspaceView();
        }
    }
}