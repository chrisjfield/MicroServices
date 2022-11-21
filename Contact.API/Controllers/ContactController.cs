namespace Contact.API.Controllers;

public static class ContactController
{
    public static RouteGroupBuilder MapContactApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllContacts)
            .DocumentGetRequest<List<ContactRecord>>("GetAllContacts", "Gets all the contacts");

        group.MapGet("{id}", GetContact)
            .DocumentGetRequest<ContactRecord>("GetContact", "Gets a contact");

        group.MapPost("/", PostContact)
            .DocumentPostRequest<ContactRecord>("PostContact", "Creates a contact");

        group.MapPut("/", PutContact)
            .DocumentPutRequest<ContactRecord>("PutContact", "Updates a contact");

        group.MapDelete("/{id}", DeleteContact)
            .DocumentDeleteRequest("DeleteContact", "Deletes a contact");

        return group;
    }

    internal static async Task<IResult> GetAllContacts(ContactDataService contactDataService) => await contactDataService.GetAllContacts();
    internal static async Task<IResult> GetContact(ContactDataService contactDataService, int id) => await contactDataService.GetContact(id);
    internal static async Task<IResult> PostContact(ContactDataService contactDataService, ContactRecord contact) => await contactDataService.PostContact(contact);
    internal static async Task<IResult> PutContact(ContactDataService contactDataService, ContactRecord contact, int id) => await contactDataService.PutContact(contact);
    internal static async Task<IResult> DeleteContact(ContactDataService contactDataService, int id) => await contactDataService.DeleteContact(id);
}
