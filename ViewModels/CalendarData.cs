using System.Linq;
using System.Collections.ObjectModel;
using TienIchLich.Models;
using Microsoft.EntityFrameworkCore;

namespace TienIchLich.ViewModels
{
    public class CalendarData : ViewModelBase
    {
        ObservableCollection<CalendarEventViewModel> calendarEvents = new();
        ObservableCollection<CalendarCategoryViewModel> calendarCategories = new();

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

        public CalendarData()
        {
            using (var db = new CalendarDbContext())
            {
                foreach (CalendarCategory categoryModel in db.CalendarCategories)
                    this.CalendarCategories.Add(GetVMFromCalendarCategoryModel(categoryModel));
                foreach (CalendarEvent calendarEvent in db.CalendarEvents)
                    this.CalendarEvents.Add(GetVMFromCalendarEventModel(calendarEvent));
            }
        }

        private CalendarEventViewModel GetVMFromCalendarEventModel(CalendarEvent calendarEvent)
        {
            CalendarCategoryViewModel categoryVM = this.calendarCategories
                .Where(b => b.Id == calendarEvent.CalendarCategoryId)
                .FirstOrDefault();
            return new CalendarEventViewModel
            {
                Subject = calendarEvent.Subject,
                StartTime = calendarEvent.StartTime,
                EndTime = calendarEvent.EndTime,
                ReminderTime = calendarEvent.ReminderTime,
                AllDay = calendarEvent.AllDay,
                Description = calendarEvent.Description,
                CalendarCategory = categoryVM
            };
        }

        private CalendarCategoryViewModel GetVMFromCalendarCategoryModel(CalendarCategory calendarCategory)
        {
            return new CalendarCategoryViewModel
            {
                Id = calendarCategory.CalendarCategoryId,
                Name = calendarCategory.Name,
                DisplayColor = calendarCategory.DisplayColor
            };
        }

        public void AddCalendarEvent(CalendarEventViewModel calendarEvent)
        {
            using (var db = new CalendarDbContext())
            {
                var newEvent = new CalendarEvent()
                {
                    Subject = calendarEvent.Subject,
                    StartTime = calendarEvent.StartTime,
                    EndTime = calendarEvent.EndTime,
                    ReminderTime = calendarEvent.ReminderTime,
                    AllDay = calendarEvent.AllDay,
                    Description = calendarEvent.Description
                };

                CalendarCategory category = db.CalendarCategories
                    .Include(i => i.Events)
                    .Single(i => i.CalendarCategoryId == calendarEvent.CalendarCategory.Id);
                category.Events.Add(newEvent);
                db.SaveChanges();
            }

            // Add to view model
            this.CalendarEvents.Add(calendarEvent);
        }

        public void EditCalendarEvent(CalendarEventViewModel calendarEvent)
        {
            using (var db = new CalendarDbContext())
            {
                CalendarEvent eventToEdit = db.CalendarEvents.Find(calendarEvent.Id);
                eventToEdit.Subject = calendarEvent.Subject;
                eventToEdit.StartTime = calendarEvent.StartTime;
                eventToEdit.EndTime = calendarEvent.EndTime;
                eventToEdit.ReminderTime = calendarEvent.ReminderTime;
                eventToEdit.AllDay = calendarEvent.AllDay;
                eventToEdit.Description = calendarEvent.Description;

                CalendarCategory category = db.CalendarCategories
                    .Include(i => i.Events)
                    .Single(i => i.CalendarCategoryId == calendarEvent.CalendarCategory.Id);
                eventToEdit.CalendarCategory = category;
                db.SaveChanges();
            }
        }

        public void DeleteCalendarEvent(CalendarEventViewModel calendarEvent)
        {
            using (var db = new CalendarDbContext())
            {
                CalendarEvent eventToDelete = db.CalendarEvents.Find(calendarEvent.Id);
                db.Remove(eventToDelete);
                db.SaveChanges();
            }
            // Delete from view model
            this.CalendarEvents.Remove(calendarEvent);
        }

        public void AddCalendarCategory(CalendarCategoryViewModel calendarCategory)
        {
            using (var db = new CalendarDbContext())
            {
                var newCategory = new CalendarCategory()
                {
                    Name = calendarCategory.Name,
                    DisplayColor = calendarCategory.DisplayColor
                };

                db.CalendarCategories.Add(newCategory);
                db.SaveChanges();
            }

            // Add to view model
            this.CalendarCategories.Add(calendarCategory);
        }

        public void EditCalendarCategory(CalendarCategoryViewModel calendarCategory)
        {
            using (var db = new CalendarDbContext())
            {
                CalendarCategory categoryToEdit = db.CalendarCategories.Find(calendarCategory.Id);
                categoryToEdit.CalendarCategoryId = calendarCategory.Id;
                categoryToEdit.DisplayColor = calendarCategory.DisplayColor;

                db.SaveChanges();
            }
        }

        public void DeleteCalendarCategory(CalendarCategoryViewModel calendarCategory)
        {
            using (var db = new CalendarDbContext())
            {
                CalendarCategory categoryToEdit = db.CalendarCategories.Find(calendarCategory.Id);
                db.CalendarCategories.Remove(categoryToEdit);
                db.SaveChanges();
            }

            // Add to view model
            this.CalendarCategories.Remove(calendarCategory);
        }
    }
}
