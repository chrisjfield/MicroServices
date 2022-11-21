using Contact.API.Controllers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ContactTests;

[TestClass]
public class ContactControllerTests
{
    private readonly DbContextOptions<ContactDb> options = new DbContextOptionsBuilder<ContactDb>()
            .UseInMemoryDatabase(databaseName: "ApplicationDb")
            .Options;

    private readonly ContactBaseRecordValidator mockValidator = new();
    private readonly NullLogger<ContactDataService> mockLogger = new();

    private readonly ContactRecord contact1 = new(1, "Chris Field");
    private readonly ContactRecord contact2 = new(2, "Fhris Cield");
    private readonly ContactBaseRecord contactPost = new("Chris Field");


    [TestMethod]
    public async Task GetAllContacts()
    {
        List<ContactRecord> contacts = new()
        {
            contact1,
            contact2,
        };

        ContactDb context = new(options);
        context.Database.EnsureDeleted();

        context.Contacts.AddRange(contacts);
        context.SaveChanges();

        ContactDataService contactDataService = new(context, mockLogger);

        var result = (Ok<List<ContactRecord>>)await ContactController.GetAllContacts(contactDataService);

        Assert.AreEqual(200, result.StatusCode);
        CollectionAssert.AreEqual(contacts, result.Value);
    }

    [TestMethod]
    public async Task GetContactById()
    {
        List<ContactRecord> contacts = new()
        {
            contact1,
            contact2,
        };

        ContactDb context = new(options);
        context.Database.EnsureDeleted();

        context.Contacts.AddRange(contacts);
        context.SaveChanges();

        ContactDataService contactDataService = new(context, mockLogger);

        var result = (Ok<ContactRecord>)await ContactController.GetContact(contactDataService, 1);

        Assert.AreEqual(200, result.StatusCode);
        Assert.AreEqual(contact1, result.Value);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException), "Invalid Id - Contact not found")]
    public async Task GetContactById_InvalidId()
    {
        List<ContactRecord> contacts = new()
        {
            contact1,
            contact2,
        };

        ContactDb context = new(options);
        context.Database.EnsureDeleted();

        context.Contacts.AddRange(contacts);
        context.SaveChanges();

        ContactDataService contactDataService = new(context, mockLogger);

        await ContactController.GetContact(contactDataService, 3);

        Assert.Fail();
    }

    [TestMethod]
    public async Task PostContact()
    {
        ContactDb context = new(options);
        context.Database.EnsureDeleted();

        ContactDataService contactDataService = new(context, mockLogger);

        var result = (Created<ContactRecord>)await ContactController.PostContact(contactDataService, mockValidator, contactPost);

        Assert.AreEqual(201, result.StatusCode);
        Assert.AreEqual(contact1, result.Value);
    }
}

