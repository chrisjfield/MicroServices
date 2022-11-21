namespace Contact.Infrastructure.Interfaces;

public interface IContactDataService
{
    Task<List<ContactRecord>> GetAll();

    Task<ContactRecord> Get(int id);

    Task<ContactRecord> Create(ContactBaseRecord contact);

    Task<ContactRecord> Update(ContactBaseRecord updateContact, int id);

    Task Delete(int id);
}