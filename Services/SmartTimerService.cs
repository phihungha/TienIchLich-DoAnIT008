using System;
using System.Collections.Generic;
using System.Timers;

namespace TienIchLich.Services
{
    /// <summary>
    /// A timer that can start only when the elapsed time is near.
    /// </summary>
    public class SmartTimer : Timer
    {
        public static readonly TimeSpan MAXIMUM_INTERVAL = new TimeSpan(0, 30, 0);

        // Id of this timer
        private int id;

        private DateTime elapsedTime = DateTime.Now;

        /// <summary>
        /// Elapsed time of timer.
        /// </summary>
        public DateTime ElapsedTime
        {
            get
            {
                return elapsedTime;
            }
            set
            {
                elapsedTime = value;
                Refresh();
            }
        }

        public SmartTimer(int id)
        {
            this.id = id;
            AutoReset = false;
            ElapsedTime = DateTime.Now;
        }

        /// <summary>
        /// Start this timer when the elapsed time is near.
        /// </summary>
        public void Refresh()
        {
            TimeSpan interval = ElapsedTime - DateTime.Now;
            if (interval <= MAXIMUM_INTERVAL && interval.TotalMilliseconds > 0)
            {
                Interval = interval.TotalMilliseconds;
                Enabled = true;
            }
        }

        /// <summary>
        /// Dispose this timer and remove it from timer service's management.
        /// </summary>
        public new void Dispose()
        {
            SmartTimerService.Timers.Remove(id);
            base.Dispose();
        }
    }

    /// <summary>
    /// Manages smart timers.
    /// </summary>
    public static class SmartTimerService
    {
        public static readonly Dictionary<int, SmartTimer> Timers = new();

        public static readonly TimeSpan REFRESH_INTERVAL = new TimeSpan(0, 30, 0);

        // Timer to refresh all smart timers periodically.
        private static Timer refreshTimer = new()
        {
            AutoReset = true,
            Interval = REFRESH_INTERVAL.TotalMilliseconds,
            Enabled = true,
        };

        // Id of next timer to be added.
        private static int nextId = 0;

        static SmartTimerService()
        {
            refreshTimer.Elapsed += RefreshTimer_Elapsed;
        }

        /// <summary>
        /// Get a smart timer.
        /// </summary>
        /// <param name="elapsedTime">Time when timer elapses</param>
        /// <returns>A smart timer instance</returns>
        public static SmartTimer GetTimer()
        {
            Timers.Add(nextId, new SmartTimer(nextId));
            nextId++;
            return Timers[nextId - 1];
        }

        /// <summary>
        /// Refresh smart timers periodically.
        /// </summary>
        private static void RefreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (SmartTimer timer in Timers.Values)
                timer.Refresh();
        }
    }
}