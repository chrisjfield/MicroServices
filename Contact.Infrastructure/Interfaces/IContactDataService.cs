namespace Contact.Infrastructure.Interfaces;

public interface IContactDataService
{
    Task<List<ContactRecord>> GetAll();

    Task<ContactRecord> Get(int id);

    Task<ContactRecord> Create(ContactRecord contact);

    Task<ContactRecord> Update(ContactRecord updateContact);

    Task Delete(int id);
}