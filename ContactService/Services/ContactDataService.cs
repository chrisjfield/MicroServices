namespace ContactService.Services
{
    public class ContactDataService
    {
        private readonly ContactDb contactDb;

        public ContactDataService(ContactDb contactDb)
        {
            this.contactDb = contactDb;
        }
        
        public async Task<IResult> GetAllContacts()
        {
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
            await contactDb.Contacts.AddAsync(contact);
            await contactDb.SaveChangesAsync();
            return Results.Created($"/contact/{contact.Id}", contact);
        }

        public async Task<IResult> PutContact(ContactRecord updateContact, int id)
        {
            if (updateContact.Id != id) return Results.BadRequest();
            
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
