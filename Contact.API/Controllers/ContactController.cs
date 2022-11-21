using Contact.Infrastructure.Extensions;
using Contact.Infrastructure.Filters;
using FluentValidation;

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
            .AddEndpointFilter<ValidationFilter<ContactRecord>>()
            .DocumentPostRequest<ContactRecord>("PostContact", "Creates a contact");

        group.MapPut("/", PutContact)
            .AddEndpointFilter<ValidationFilter<ContactRecord>>()
            .DocumentPutRequest<ContactRecord>("PutContact", "Updates a contact");

        group.MapDelete("/{id}", DeleteContact)
            .DocumentDeleteRequest("DeleteContact", "Deletes a contact");

        return group;
    }

    internal static async Task<IResult> GetAllContacts(ContactDataService contactDataService) =>
        Results.Ok(await contactDataService.GetAllContacts());

    internal static async Task<IResult> GetContact(ContactDataService contactDataService, int id) => 
        Results.Ok(await contactDataService.GetContact(id));

    internal static async Task<IResult> PostContact(ContactDataService contactDataService, IValidator<ContactRecord> validator, ContactRecord contact) => 
        await contactDataService.PostContact(contact);

    internal static async Task<IResult> PutContact(ContactDataService contactDataService, IValidator<ContactRecord> validator, ContactRecord contact, int id) => 
        await contactDataService.PutContact(contact);

    internal static async Task<IResult> DeleteContact(ContactDataService contactDataService, int id) => 
        await contactDataService.DeleteContact(id);
}
