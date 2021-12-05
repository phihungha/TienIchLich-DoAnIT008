﻿using Microsoft.EntityFrameworkCore;
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
        private ReminderManager reminderManager = new();

        private ObservableCollection<CalendarEventVM> calendarEventVMs = new();

        /// <summary>
        /// Calendar event view model collection.
        /// </summary>
        public ObservableCollection<CalendarEventVM> CalendarEventVMs => calendarEventVMs;

        public CalendarEventVMManager(NavigationVM navigationVM, CalendarCategoryVMManager categoryVMs)
        {
            this.navigationVM = navigationVM;
            this.categoryVMs = categoryVMs;

            // Build view models from all calendar events in database
            using (var db = new CalendarDbContext())
                foreach (CalendarEvent calendarEvent in db.CalendarEvents)
                    this.CalendarEventVMs.Add(GetVMFromCalendarEventModel(calendarEvent));
        }

        /// <summary>
        /// Build view model of calendar event.
        /// </summary>
        /// <param name="calendarEvent">Calendar event model object.</param>
        /// <returns></returns>
        private CalendarEventVM GetVMFromCalendarEventModel(CalendarEvent calendarEvent)
        {
            CalendarCategoryVM categoryVM = this.categoryVMs.CalendarCategoryVMs
                .Where(i => i.Id == calendarEvent.CalendarCategoryId)
                .FirstOrDefault();
            var eventVM = new CalendarEventVM(this.navigationVM)
            {
                Id = calendarEvent.CalendarEventId,
                Subject = calendarEvent.Subject,
                StartTime = calendarEvent.StartTime,
                EndTime = calendarEvent.EndTime,
                ReminderTime = calendarEvent.ReminderTime,
                AllDay = calendarEvent.AllDay,
                Description = calendarEvent.Description,
                CalendarCategoryVM = categoryVM
            };
            reminderManager.Add(
                eventVM.Id, 
                eventVM.StartTime, 
                eventVM.ReminderTime, 
                eventVM.ReminderTimer_Elapsed);
            return eventVM;
        }

        /// <summary>
        /// Add new calendar event to database from provided calendar event view model.
        /// </summary>
        /// <param name="eventVM">View model of calendar event to add.</param>
        public void AddCalendarEvent(CalendarEventVM eventVM)
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
                    .Single(i => i.CalendarCategoryId == eventVM.CalendarCategoryVM.Id);
                category.Events.Add(newEvent);
                db.SaveChanges();
                eventVM.Id = newEvent.CalendarEventId;
            }

            this.CalendarEventVMs.Add(eventVM);

            reminderManager.Add(
                eventVM.Id,
                eventVM.StartTime,
                eventVM.ReminderTime,
                eventVM.ReminderTimer_Elapsed);
        }

        /// <summary>
        /// Edit calendar event of provided calendar event view model in database.
        /// </summary>
        /// <param name="eventVM">View model of calendar event to edit.</param>
        public void EditCalendarEvent(CalendarEventVM eventVM)
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
                    .Single(i => i.CalendarCategoryId == eventVM.CalendarCategoryVM.Id);
                eventToEdit.CalendarCategory = category;

                db.SaveChanges();
            }

            reminderManager.Edit(eventVM.Id, eventVM.StartTime, eventVM.ReminderTime);
        }

        /// <summary>
        /// Delete calendar event of provided calendar event view model from database.
        /// </summary>
        /// <param name="eventVM">View model of calendar event to delete.</param>
        public void DeleteCalendarEvent(CalendarEventVM eventVM)
        {
            using (var db = new CalendarDbContext())
            {
                CalendarEvent eventToDelete = db.CalendarEvents.Single(i => i.CalendarEventId == eventVM.Id);
                db.Remove(eventToDelete);
                db.SaveChanges();
            }

            this.CalendarEventVMs.Remove(eventVM);

            reminderManager.Remove(eventVM.Id);
        }
    }
}