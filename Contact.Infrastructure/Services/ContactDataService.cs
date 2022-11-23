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

    public async Task<ContactRecord> Create(ContactBaseRecord contact)
    {
        logger.LogInformation("Contact posted: {contact}", contact);

        var nextId = contactDb.Contacts.DefaultIfEmpty().Max(c => c == null ? 1 : c.Id + 1);

        var (name, gender, dateOfBirth) = contact;
        ContactRecord contactToPost = new(nextId, name, gender, dateOfBirth);

        await contactDb.Contacts.AddAsync(contactToPost);
        int res = await contactDb.SaveChangesAsync();

        if (res != 1) throw new Exception();

        return contactToPost;
    }

    public async Task<ContactRecord> Update(ContactBaseRecord updateContact, int id)
    {
        ContactRecord? contact = await contactDb.Contacts.AsNoTracking().FirstOrDefaultAsync((c) => c.Id == id);
        if (contact is null) throw new KeyNotFoundException($"A record with the id: {id} was not found");

        var (name, gender, dateOfBirth) = updateContact;
        ContactRecord contactToUpdate = contact with { Name = name, Gender = gender, DateOfBirth = dateOfBirth };

        contactDb.Contacts.Update(contactToUpdate);
        int res = await contactDb.SaveChangesAsync();

        if (res != 1) { throw new Exception(); }

        return contactToUpdate;
    }

    public async Task<ContactRecord> Patch(ContactBaseRecord updateContact, int id)
    {
        ContactRecord? contact = await contactDb.Contacts.AsNoTracking().FirstOrDefaultAsync((c) => c.Id == id);
        if (contact is null) throw new KeyNotFoundException($"A record with the id: {id} was not found");

        var (name, gender, dateOfBirth) = updateContact;
        ContactRecord contactToUpdate = contact with { 
            Name = name ?? contact.Name, 
            Gender = gender ?? contact.Gender, 
            DateOfBirth = dateOfBirth ?? contact.DateOfBirth,
        };

        contactDb.Contacts.Update(contactToUpdate);
        int res = await contactDb.SaveChangesAsync();

        if (res != 1) { throw new Exception(); }

        return contactToUpdate;
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
