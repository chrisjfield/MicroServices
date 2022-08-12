using Microsoft.Extensions.Logging.Abstractions;
using ContactService.Infrastructure;
using ContactService.Services;
using ContactService.Models;

namespace UnitTests
{

    [TestClass]
    public class ContactServiceTests
    {
        private readonly DbContextOptions<ContactDb> options = new DbContextOptionsBuilder<ContactDb>()
                .UseInMemoryDatabase(databaseName: "ApplicationDb")
                .Options;

        private readonly ContactRecordValidator mockValidator = new();
        private readonly NullLogger<ContactDataService> mockLogger = new();

        [TestMethod]
        public async Task GetAllContacts()
        {
            List<ContactRecord> contacts = new()
            {
                new (1, "Chris Field"),
                new (2, "Fhris Cield"),
            };

            ContactDb context = new(options);
            context.Contacts.AddRange(contacts);
            context.SaveChanges();
          
            ContactDataService contactDataService = new(context, mockValidator, mockLogger);

            IResult result = await contactDataService.GetAllContacts();

            var (response, body) = await TestHelper.DeconstructIResult<List<ContactRecord>>(result);

            Assert.AreEqual(200, response.StatusCode);
            CollectionAssert.AreEqual(contacts, body);

            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task GetContactById()
        {
            List<ContactRecord> contacts = new()
            {
                new (1, "Chris Field"),
                new (2, "Fhris Cield"),
            };

            ContactDb context = new(options);
            context.Contacts.AddRange(contacts);
            context.SaveChanges();

            ContactDataService contactDataService = new(context, mockValidator, mockLogger);

            IResult result = await contactDataService.GetContact(1);

            var (response, body) = await TestHelper.DeconstructIResult<ContactRecord>(result);

            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual(contacts[0], body);

            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task GetContactById_InvalidId()
        {
            List<ContactRecord> contacts = new()
            {
                new (1, "Chris Field"),
                new (2, "Fhris Cield"),
            };

            ContactDb context = new(options);
            context.Contacts.AddRange(contacts);
            context.SaveChanges();

            ContactDataService contactDataService = new(context, mockValidator, mockLogger);

            IResult result = await contactDataService.GetContact(3);

            var (response, body) = await TestHelper.DeconstructIResult<ContactRecord>(result);

            Assert.AreEqual(404, response.StatusCode);
            Assert.IsNull(body);

            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task PostContact()
        {
            ContactRecord contact = new(1, "Chris Field");

            ContactDb context = new(options);

            ContactDataService contactDataService = new(context, mockValidator, mockLogger);

            IResult result = await contactDataService.PostContact(contact);

            var (response, body) = await TestHelper.DeconstructIResult<ContactRecord>(result);

            Assert.AreEqual(201, response.StatusCode);
            Assert.AreEqual(contact, body);

            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task PostContact_InvalidGender()
        {
            ContactRecord contact = new(1, "Chris Field", "NotAGender");

            ContactDb context = new(options);

            ContactDataService contactDataService = new(context, mockValidator, mockLogger);

            IResult result = await contactDataService.PostContact(contact);

            var (response, body) = await TestHelper.DeconstructIResult<ValidationErrors>(result);

            Assert.AreEqual(400, response.StatusCode);
            Assert.AreEqual(1, body?.Errors.Count);
            Assert.AreEqual("Please only use: Female,Male,Other", body?.Errors[0]);

            context.Database.EnsureDeleted();
        }
    }
}