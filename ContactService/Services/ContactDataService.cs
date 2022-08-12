namespace ContactService.Services
{
    public class ContactDataService
    {
        private readonly ContactDb contactDb;
        private readonly IValidator<ContactRecord> validator;

        public ContactDataService(ContactDb contactDb, IValidator<ContactRecord> validator)
        {
            this.contactDb = contactDb;
            this.validator = validator;
        }

        public async Task<IResult> GetAllContacts()
        {
            throw new AppException("custom error");
            List<ContactRecord> contacts = await contactDb.Contacts.ToListAsync();
            return Results.Ok(contacts);
        }

        public async Task<IResult> GetContact(int id)
        {
            ContactRecord? contact = await contactDb.Contacts.FindAsync(id);
            
            if (contact == null) { return Results.NotFound(); }
            return Results.Ok(contact);
        }

        public async Task<IResult> PostContact(ContactRecord contact)
        {
            ValidationErrors? modelValidation = validator.ValidateRecord(contact);
            if (modelValidation != null) { return Results.BadRequest(modelValidation); }

            await contactDb.Contacts.AddAsync(contact);
            await contactDb.SaveChangesAsync();
            return Results.Created($"/contact/{contact.Id}", contact);
        }

        public async Task<IResult> PutContact(ContactRecord updateContact, int id)
        {
            if (updateContact.Id != id) return Results.BadRequest(new ValidationErrors(new() { "'Id' must match the Id value in the request body" }));

            ValidationErrors? modelValidation = validator.ValidateRecord(updateContact);
            if (modelValidation != null) { return Results.BadRequest(modelValidation); }

            ContactRecord? contact = await contactDb.Contacts.AsNoTracking().FirstOrDefaultAsync((c) => c.Id == id);
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
}
