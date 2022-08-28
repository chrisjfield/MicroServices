namespace Contact.Infrastructure.Services;

public class ContactDataService
{
    private readonly ContactDb contactDb;
    private readonly IValidator<ContactRecord> validator;
    private readonly ILogger<ContactDataService> logger;

    public ContactDataService(ContactDb contactDb, IValidator<ContactRecord> validator, ILogger<ContactDataService> logger)
    {
        this.contactDb = contactDb;
        this.validator = validator;
        this.logger = logger;
    }

    public async Task<IResult> GetAllContacts()
    {
        List<ContactRecord> contacts = await contactDb.Contacts.ToListAsync();
        return Results.Ok(contacts);
    }

    public async Task<IResult> GetContact(int id)
    {
        ContactRecord? contact = await contactDb.Contacts.FindAsync(id);

        if (contact == null) throw new KeyNotFoundException($"a record with the id: {id} was not found");
        return Results.Ok(contact);
    }

    public async Task<IResult> PostContact(ContactRecord contact)
    {
        logger.LogInformation("Contact posted: {contact}", contact);

        ValidationErrors? modelValidation = validator.ValidateRecord(contact);
        if (modelValidation != null) { return Results.BadRequest(modelValidation); }

        await contactDb.Contacts.AddAsync(contact);
        await contactDb.SaveChangesAsync();
        return Results.Created($"/contact/{contact.Id}", contact);
    }

    public async Task<IResult> PutContact(ContactRecord updateContact)
    {
        ValidationErrors? modelValidation = validator.ValidateRecord(updateContact);
        if (modelValidation != null) { return Results.BadRequest(modelValidation); }

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
