namespace Contact.Infrastructure.Services;

public class ContactDataService
{
    private readonly ContactDb contactDb;
    private readonly ILogger<ContactDataService> logger;

    public ContactDataService(ContactDb contactDb, ILogger<ContactDataService> logger)
    {
        this.contactDb = contactDb;
        this.logger = logger;
    }

    public async Task<List<ContactRecord>> GetAllContacts()
    {
        return await contactDb.Contacts.ToListAsync();
    }

    public async Task<ContactRecord> GetContact(int id)
    {
        ContactRecord? contact = await contactDb.Contacts.FindAsync(id);

        if (contact == null) throw new KeyNotFoundException($"a record with the id: {id} was not found");
        
        return contact;
    }

    public async Task<IResult> PostContact(ContactRecord contact)
    {
        logger.LogInformation("Contact posted: {contact}", contact);

        await contactDb.Contacts.AddAsync(contact);
        await contactDb.SaveChangesAsync();
        return Results.Created($"/contact/{contact.Id}", contact);
    }

    public async Task<IResult> PutContact(ContactRecord updateContact)
    {
        ContactRecord? contact = await contactDb.Contacts.AsNoTracking().FirstOrDefaultAsync((c) => c.Id == updateContact.Id);
        if (contact is null) return Results.NotFound();

        contactDb.Contacts.Update(updateContact);
        await contactDb.SaveChangesAsync();
        return Results.Ok(updateContact);
    }

    public async Task<IResult> DeleteContact(int id)
    {
        ContactRecord? contact = await contactDb.Contacts.FindAsync(id);
        if (contact is null) return Results.NotFound();

        contactDb.Contacts.Remove(contact);
        await contactDb.SaveChangesAsync();
        return Results.NoContent();
    }
}
