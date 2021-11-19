using Microsoft.EntityFrameworkCore;

namespace TienIchLich.Models
{
    /// <summary>
    /// Database context for calendar events.
    /// </summary>
    public class CalendarData : DbContext
    {
        /// <summary>
        /// Calendar categories.
        /// </summary>
        public DbSet<CalendarCategory> CalendarCategories { get; set; }

        /// <summary>
        /// Calendar events.
        /// </summary>
        public DbSet<CalendarEvent> CalendarEvents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=CalendarData.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map TimeSpan of ReminderTime to text in database.
            modelBuilder.Entity<CalendarEvent>()
                .Property(e => e.ReminderTime)
                .HasConversion<string>();
        }
    }
}
