using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TienIchLich.Models;

namespace TienIchLich.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<CalendarEventViewModel> calendarEvents = new ObservableCollection<CalendarEventViewModel>();
        private ObservableCollection<CalendarCategoryViewModel> calendarCategories = new ObservableCollection<CalendarCategoryViewModel>();

        /// <summary>
        /// Calendar events.
        /// </summary>
        public ObservableCollection<CalendarEventViewModel> CalendarEvents
        {
            get
            {
                return calendarEvents;
            }
            set
            {
                calendarEvents = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Calendar categories.
        /// </summary>
        public ObservableCollection<CalendarCategoryViewModel> CalendarCategories
        {
            get
            {
                return calendarCategories;
            }
            set
            {
                CalendarCategories = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Raises property change event.
        /// </summary>
        /// <param name="propertyName"></param>
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public MainViewModel()
        {
            using (var dbContext = new CalendarData())
            {
                dbContext.Database.EnsureCreated();

                CalendarEvent calendarEvent = dbContext.CalendarEvents.First();
                var newEvent = new CalendarEventViewModel()
                {
                    Subject = calendarEvent.Subject,
                    Description = calendarEvent.Subject,
                    StartTime = calendarEvent.StartTime,
                    EndTime = calendarEvent.EndTime,
                    AllDay = calendarEvent.AllDay,
                    ReminderTime = calendarEvent.ReminderTime
                };
                calendarEvents.Add(newEvent);
                CalendarCategory calendarCategory = dbContext.CalendarCategories.First();

                var newCalendarCategory = new CalendarCategoryViewModel()
                {
                    Name = calendarCategory.Name,
                    DisplayColor = calendarCategory.DisplayColor,
                };
                calendarCategories.Add(newCalendarCategory);
            }
        }
    }
}
