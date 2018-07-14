using System.Data.Entity;

namespace MyEventCalendar.Models
{
    public class EventDbContext:DbContext
    {
        public EventDbContext() : base("EventConnection")
        {
            Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<EventModel> EventModels { get; set; }
    }
}