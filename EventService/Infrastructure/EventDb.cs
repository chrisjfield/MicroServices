namespace EventService.Infrastructure
{
    public class EventDb : DbContext
    {
        public EventDb(DbContextOptions options) : base(options) { }
        public DbSet<EventRecord> Events { get; set; } = null!;
    }
}
