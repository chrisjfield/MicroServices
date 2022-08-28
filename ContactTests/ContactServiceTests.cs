namespace ContactTests;

[TestClass]
public class ContactServiceTests
{
    private readonly DbContextOptions<ContactDb> options = new DbContextOptionsBuilder<ContactDb>()
            .UseInMemoryDatabase(databaseName: "ApplicationDb")
            .Options;

    private readonly ContactRecordValidator mockValidator = new();
    private readonly NullLogger<ContactDataService> mockLogger = new();

    private readonly ContactRecord contact1 = new(1, "Chris Field");
    private readonly ContactRecord contact2 = new(2, "Fhris Cield");


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

        ContactDataService contactDataService = new(context, mockValidator, mockLogger);

        IResult result = await contactDataService.GetAllContacts();

        var (response, body) = await TestHelper.DeconstructIResult<List<ContactRecord>>(result);

        Assert.AreEqual(200, response.StatusCode);
        CollectionAssert.AreEqual(contacts, body);
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

        ContactDataService contactDataService = new(context, mockValidator, mockLogger);

        IResult result = await contactDataService.GetContact(1);

        var (response, body) = await TestHelper.DeconstructIResult<ContactRecord>(result);

        Assert.AreEqual(200, response.StatusCode);
        Assert.AreEqual(contact1, body);
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

        ContactDataService contactDataService = new(context, mockValidator, mockLogger);

        await contactDataService.GetContact(3);

        Assert.Fail();
    }

    [TestMethod]
    public async Task PostContact()
    {
        ContactDb context = new(options);
        context.Database.EnsureDeleted();

        ContactDataService contactDataService = new(context, mockValidator, mockLogger);

        IResult result = await contactDataService.PostContact(contact1);

        var (response, body) = await TestHelper.DeconstructIResult<ContactRecord>(result);

        Assert.AreEqual(201, response.StatusCode);
        Assert.AreEqual(contact1, body);
    }

    [TestMethod]
    public async Task PostContact_InvalidGender()
    {
        ContactRecord contact = contact1 with { Gender = "NotAGender" };

        ContactDb context = new(options);
        context.Database.EnsureDeleted();

        ContactDataService contactDataService = new(context, mockValidator, mockLogger);

        IResult result = await contactDataService.PostContact(contact);

        var (response, body) = await TestHelper.DeconstructIResult<ValidationErrors>(result);

        Assert.AreEqual(400, response.StatusCode);
        Assert.AreEqual(1, body?.Errors.Count);
        Assert.AreEqual("Please only use: Female,Male,Other", body?.Errors[0]);
    }
}

