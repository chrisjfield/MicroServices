namespace Contact.API;

public static class EndpointDefinitions
{
    public static void AddEndpoints(this WebApplication app)
    {
        app.MapGet("/contact", GetAllContacts)
            .DocumentGetRequest<List<ContactRecord>>("Contacts", "GetAllContacts", "Gets all the contacts");

        app.MapGet("/contact/{id}", GetContact)
            .DocumentGetRequest<ContactRecord>("Contact", "GetContact", "Gets a contact");

        app.MapPost("/contact", PostContact)
            .DocumentPostRequest<ContactRecord>("Contact", "PostContact", "Creates a contact");

        app.MapPut("/contact", PutContact)
            .DocumentPutRequest<ContactRecord>("Contact", "PutContact", "Updates a contact");

        app.MapDelete("/contact/{id}", DeleteContact)
            .DocumentDeleteRequest("Contact", "DeleteContact", "Deletes a contact");

        // Exception sample
        app.MapGet("/contact/{id}/throw", ThrowInvalidOpException)
            .DocumentGetRequest<ContactRecord>("Contact", "GetContactThrow", "Throws demo exception");

    }

    internal static async Task<IResult> GetAllContacts(ContactDataService contactDataService) => await contactDataService.GetAllContacts();
    internal static async Task<IResult> GetContact(ContactDataService contactDataService, int id) => await contactDataService.GetContact(id);
    internal static async Task<IResult> PostContact(ContactDataService contactDataService, ContactRecord contact) => await contactDataService.PostContact(contact);
    internal static async Task<IResult> PutContact(ContactDataService contactDataService, ContactRecord contact, int id) => await contactDataService.PutContact(contact);
    internal static async Task<IResult> DeleteContact(ContactDataService contactDataService, int id) => await contactDataService.DeleteContact(id);

    internal static Task<IResult> ThrowInvalidOpException(int id) => throw new InvalidOperationException($"This is a sample request that throws exception with contact {id}");
}
