namespace Contact.Infrastructure.Services;

public class ContactDataService : IContactDataService
{
    private readonly ContactDb contactDb;
    private readonly ILogger<ContactDataService> logger;

    public ContactDataService(ContactDb contactDb, ILogger<ContactDataService> logger)
    {
        this.contactDb = contactDb;
        this.logger = logger;
    }

    public async Task<List<ContactRecord>> GetAll()
    {
        return await contactDb.Contacts.ToListAsync();
    }

    public async Task<ContactRecord> Get(int id)
    {
        ContactRecord? contact = await contactDb.Contacts.FindAsync(id);

        if (contact == null) throw new KeyNotFoundException($"A record with the id: {id} was not found");
        
        return contact;
    }

    public async Task<ContactRecord> Create(ContactRecord contact)
    {
        logger.LogInformation("Contact posted: {contact}", contact);

        await contactDb.Contacts.AddAsync(contact);
        int res = await contactDb.SaveChangesAsync();

        if (res != 1) throw new Exception();

        return contact;
    }

    public async Task<ContactRecord> Update(ContactRecord updateContact)
    {
        ContactRecord? contact = await contactDb.Contacts.AsNoTracking().FirstOrDefaultAsync((c) => c.Id == updateContact.Id);
        if (contact is null) throw new KeyNotFoundException($"A record with the id: {updateContact.Id} was not found");

        contactDb.Contacts.Update(updateContact);
        int res = await contactDb.SaveChangesAsync();

        if (res != 1) { throw new Exception(); }

        return updateContact;
    }

    public async Task Delete(int id)
    {
        ContactRecord? contact = await contactDb.Contacts.FindAsync(id);
        if (contact is null) throw new KeyNotFoundException($"A record with the id: {id} was not found");

        contactDb.Contacts.Remove(contact);
        int res = await contactDb.SaveChangesAsync();

        if (res != 1) { throw new Exception(); }
    }
}
