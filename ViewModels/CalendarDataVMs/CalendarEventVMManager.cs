using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using TienIchLich.Models;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// Manager for calendar event view models.
    /// Used to access database.
    /// </summary>
    public class CalendarEventVMManager : ViewModelBase
    {
        private CalendarCategoryVMManager categoryVMs;
        private NavigationVM navigationVM;
        private ReminderManager reminderManager;

        /// <summary>
        /// Calendar event view model collection.
        /// </summary>
        public ObservableCollection<CalendarEventVM> CalendarEventVMs { get; private set; }

        public CalendarEventVMManager(NavigationVM navigationVM, CalendarCategoryVMManager categoryVMs, ReminderManager reminderManager)
        {
            this.navigationVM = navigationVM;
            this.categoryVMs = categoryVMs;
            this.reminderManager = reminderManager;
            CalendarEventVMs = new ObservableCollection<CalendarEventVM>();

            // Build view models for all calendar events in database
            using (var db = new CalendarDbContext())
                foreach (CalendarEvent calendarEvent in db.CalendarEvents)
                    CalendarEventVMs.Add(GetVMFromModel(calendarEvent));
        }

        /// <summary>
        /// Get view model of a calendar event from model data.
        /// </summary>
        /// <param name="calendarEvent">Calendar event model object</param>
        /// <returns></returns>
        private CalendarEventVM GetVMFromModel(CalendarEvent calendarEvent)
        {
            CalendarCategoryVM categoryVM = categoryVMs.CalendarCategoryVMs
                .Where(i => i.Id == calendarEvent.CalendarCategoryId)
                .FirstOrDefault();
            var eventVM = new CalendarEventVM(this, navigationVM)
            {
                Id = calendarEvent.CalendarEventId,
                Subject = calendarEvent.Subject,
                StartTime = calendarEvent.StartTime,
                EndTime = calendarEvent.EndTime,
                ReminderTime = calendarEvent.ReminderTime,
                AllDay = calendarEvent.AllDay,
                Description = calendarEvent.Description,
                CategoryVM = categoryVM
            };
            reminderManager.Add(
                eventVM.Id,
                eventVM.StartTime,
                eventVM.ReminderTime,
                eventVM.ReminderTimer_Elapsed);
            return eventVM;
        }

        /// <summary>
        /// Add a new calendar event into database.
        /// </summary>
        /// <param name="eventVM">View model of calendar event to add</param>
        public void Add(CalendarEventVM eventVM)
        {
            using (var db = new CalendarDbContext())
            {
                var newEvent = new CalendarEvent()
                {
                    Subject = eventVM.Subject,
                    StartTime = eventVM.StartTime,
                    EndTime = eventVM.EndTime,
                    ReminderTime = eventVM.ReminderTime,
                    AllDay = eventVM.AllDay,
                    Description = eventVM.Description
                };

                CalendarCategory category = db.CalendarCategories
                    .Include(i => i.Events)
                    .Single(i => i.CalendarCategoryId == eventVM.CategoryVM.Id);
                category.Events.Add(newEvent);
                db.SaveChanges();
                eventVM.Id = newEvent.CalendarEventId;
            }

            CalendarEventVMs.Add(eventVM);

            reminderManager.Add(
                eventVM.Id,
                eventVM.StartTime,
                eventVM.ReminderTime,
                eventVM.ReminderTimer_Elapsed);
        }

        /// <summary>
        /// Edit a calendar event in database.
        /// </summary>
        /// <param name="eventVM">View model of calendar event to edit</param>
        public void Edit(CalendarEventVM eventVM)
        {
            using (var db = new CalendarDbContext())
            {
                CalendarEvent eventToEdit = db.CalendarEvents.Single(i => i.CalendarEventId == eventVM.Id);
                eventToEdit.Subject = eventVM.Subject;
                eventToEdit.StartTime = eventVM.StartTime;
                eventToEdit.EndTime = eventVM.EndTime;
                eventToEdit.ReminderTime = eventVM.ReminderTime;
                eventToEdit.AllDay = eventVM.AllDay;
                eventToEdit.Description = eventVM.Description;

                CalendarCategory category = db.CalendarCategories
                    .Include(i => i.Events)
                    .Single(i => i.CalendarCategoryId == eventVM.CategoryVM.Id);
                eventToEdit.CalendarCategory = category;

                db.SaveChanges();
            }

            reminderManager.Edit(eventVM.Id, eventVM.StartTime, eventVM.ReminderTime);
        }

        /// <summary>
        /// Delete a calendar event from database.
        /// </summary>
        /// <param name="eventVM">View model of calendar event to delete</param>
        public void Delete(CalendarEventVM eventVM)
        {
            using (var db = new CalendarDbContext())
            {
                CalendarEvent eventToDelete = db.CalendarEvents.Single(i => i.CalendarEventId == eventVM.Id);
                db.Remove(eventToDelete);
                db.SaveChanges();
            }

            CalendarEventVMs.Remove(eventVM);

            reminderManager.Remove(eventVM.Id);
        }
    }
}