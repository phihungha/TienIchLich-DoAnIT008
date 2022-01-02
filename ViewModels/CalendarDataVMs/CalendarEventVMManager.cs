using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using TienIchLich.Models;
using TienIchLich.Services;

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
        private DialogService dialogService;

        /// <summary>
        /// Calendar event view model collection.
        /// </summary>
        public ObservableCollection<CalendarEventVM> CalendarEventVMs { get; private set; } = new();

        public CalendarEventVMManager(NavigationVM navigationVM, CalendarCategoryVMManager categoryVMs, DialogService dialogService)
        {
            this.navigationVM = navigationVM;
            this.categoryVMs = categoryVMs;
            this.dialogService = dialogService;

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
            var eventVM = new CalendarEventVM(this, navigationVM, dialogService)
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

            eventVM.CategoryVM.UnregisterEvent();
            eventVM.Dispose();
            CalendarEventVMs.Remove(eventVM);
        }

        /// <summary>
        /// Delete all calendar event view models belong to a calendar category.
        /// This is used when cascade delete happens.
        /// </summary>
        /// <param name="categoryId">Id of calendar category</param>
        public void DeleteVMsOfCategory(long categoryId)
        {
            foreach (CalendarEventVM eventVM in CalendarEventVMs.ToList())
            {
                if (eventVM.CategoryVM.Id == categoryId)
                    CalendarEventVMs.Remove(eventVM);
            }
        }
    }
}