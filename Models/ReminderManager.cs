using System;
using System.Collections.Generic;
using System.Timers;

namespace TienIchLich.Models
{
    /// <summary>
    /// Manage timers that remind users when an event is approaching.
    /// </summary>
    public class ReminderManager
    {
        private Dictionary<long, Timer> reminderTimers = new();

        /// <summary>
        /// Get interval until reminding for timers.
        /// </summary>
        /// <param name="startTime">Start time of calendar event</param>
        /// <param name="reminderTime">Reminder time of calendar event</param>
        /// <returns></returns>
        private double GetReminderInterval(DateTime startTime, TimeSpan reminderTime)
            => (startTime - reminderTime - DateTime.Now).TotalMilliseconds;

        /// <summary>
        /// Add a new reminder timer. 
        /// Start the timer if there is still time until reminding.
        /// </summary>
        /// <param name="calendarEventId">Calendar event Id associated with this timer for later access</param>
        /// <param name="startTime">Start time of calendar event</param>
        /// <param name="reminderTime">Reminder time of calendar event</param>
        /// <param name="elapsedEventHandler">Method that does work to remind users</param>
        public void Add(long calendarEventId, DateTime startTime, TimeSpan reminderTime, ElapsedEventHandler elapsedEventHandler)
        {
            double timerInterval = GetReminderInterval(startTime, reminderTime);

            var timer = new Timer();
            timer.AutoReset = false;
            timer.Elapsed += elapsedEventHandler;
            reminderTimers.Add(calendarEventId, timer);

            if (timerInterval > 0)
            {
                timer.Interval = timerInterval;
                timer.Start();
            }
        }

        /// <summary>
        /// Edit a reminder timer.
        /// Start the timer if there is still time until reminding.
        /// Stop the timer if reminding time has passed.
        /// </summary>
        /// <param name="calendarEventId">Calendar event Id of timer to edit</param>
        /// <param name="startTime">Start time of calendar event</param>
        /// <param name="reminderTime">Reminder time of calendar event</param>
        public void Edit(long calendarEventId, DateTime startTime, TimeSpan reminderTime)
        {
            double timerInterval = GetReminderInterval(startTime, reminderTime);
            if (timerInterval > 0)
            {
                reminderTimers[calendarEventId].Interval = timerInterval;
                reminderTimers[calendarEventId].Start();
            }
            else
                reminderTimers[calendarEventId].Stop();
        }

        /// <summary>
        /// Stop and remove a reminder timer.
        /// </summary>
        /// <param name="calendarEventId">Calendar event Id of timer to remove.</param>
        public void Remove(long calendarEventId)
        {
            reminderTimers[calendarEventId].Stop();
            reminderTimers[calendarEventId].Dispose();
            reminderTimers.Remove(calendarEventId);
        }
    }
}
