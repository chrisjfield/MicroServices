namespace Contact.Infrastructure;

public class ContactDb : DbContext
{
    public ContactDb(DbContextOptions options) : base(options) { }
    public DbSet<ContactRecord> Contacts { get; set; } = null!;
}
