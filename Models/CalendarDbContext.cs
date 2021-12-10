using Microsoft.EntityFrameworkCore;

namespace TienIchLich.Models
{
    /// <summary>
    /// Database access context for calendar event data.
    /// </summary>
    public class CalendarDbContext : DbContext
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