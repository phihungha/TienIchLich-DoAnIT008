﻿using System;
using System.Windows.Input;
using TienIchLich.Models;
using TienIchLich.Services;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// Identifiers for remind again time options.
    /// </summary>
    public enum RemindAgainTimeOptionId
    {
        Minute5,
        Minute10,
        Minute15
    }

    /// <summary>
    /// Remind time option info.
    /// </summary>
    public struct RemindAgainTimeOption
    {
        public RemindAgainTimeOptionId Id { get; set; }
        public TimeSpan Time { get; set; }
    }

    /// <summary>
    /// View model for the reminder view.
    /// </summary>
    public class ReminderVM : ViewModelBase
    {
        private NavigationVM navigationVM;
        private ReminderManager reminderManager;
        private AlarmSoundService alarmSoundService;

        private CalendarEventVM eventVM;

        /// <summary>
        /// View model of the calendar event being reminded.
        /// </summary>
        public CalendarEventVM EventVM
        {
            get
            {
                return eventVM;
            }
            private set
            {
                eventVM = value;
                NotifyPropertyChanged();
            }
        }

        private static RemindAgainTimeOption[] remindAgainTimeOptions =
        {
            new RemindAgainTimeOption()
            {
                Id = RemindAgainTimeOptionId.Minute5,
                Time = new TimeSpan(0, 5, 0)
            },
            new RemindAgainTimeOption()
            {
                Id = RemindAgainTimeOptionId.Minute10,
                Time = new TimeSpan(0, 10, 0)
            },
            new RemindAgainTimeOption()
            {
                Id = RemindAgainTimeOptionId.Minute15,
                Time = new TimeSpan(0, 15, 0)
            }
        };

        /// <summary>
        /// Remind again time options
        /// </summary>
        public static RemindAgainTimeOption[] RemindAgainTimeOptions => remindAgainTimeOptions;

        private RemindAgainTimeOption selectedRemindAgainTimeOption = RemindAgainTimeOptions[0];

        /// <summary>
        /// Selected remind again time option.
        /// </summary>
        public RemindAgainTimeOption SelectedRemindAgainTimeOption
        {
            get
            {
                return selectedRemindAgainTimeOption;
            }
            set
            {
                selectedRemindAgainTimeOption = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Command to acknowledge the reminder and cancel it.
        /// </summary>
        public ICommand AckCommand { get; private set; }

        /// <summary>
        /// Command to remind again later.
        /// </summary>
        public ICommand RemindLaterCommand { get; private set; }

        public ReminderVM(NavigationVM navigationVM, ReminderManager reminderManager, AlarmSoundService alarmSoundService)
        {
            this.navigationVM = navigationVM;
            this.reminderManager = reminderManager;
            this.alarmSoundService = alarmSoundService;
            AckCommand = new RelayCommand(i => Acknowledge());
            RemindLaterCommand = new RelayCommand(i => RemindAgain());
        }

        /// <summary>
        /// Play alarm sound and display reminded event.
        /// </summary>
        /// <param name="eventVM"></param>
        public void Remind(CalendarEventVM eventVM)
        {
            EventVM = eventVM;
            alarmSoundService.PlaySound(@"../../../Sounds/ringtone1.mp3", 100);
        }

        /// <summary>
        /// Set the reminder timer of the displayed event to remind again at some point.
        /// </summary>
        private void RemindAgain()
        {
            reminderManager.EditInterval(eventVM.Id, SelectedRemindAgainTimeOption.Time);
            Acknowledge();
        }

        /// <summary>
        /// Stop sound and return to main workspace view.
        /// </summary>
        private void Acknowledge()
        {
            alarmSoundService.StopSound();
            navigationVM.NavigateToMainWorkspaceView();
        }
    }
}