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
            .AddEndpointFilter<ValidationFilter<ContactBaseRecord>>()
            .DocumentPostRequest<ContactRecord>("PostContact", "Creates a contact");

        group.MapPut("/{id}", PutContact)
            .AddEndpointFilter<ValidationFilter<ContactBaseRecord>>()
            .DocumentUpdateRequest<ContactRecord>("PutContact", "Updates a contact");

        group.MapDelete("/{id}", DeleteContact)
            .DocumentDeleteRequest("DeleteContact", "Deletes a contact");

        return group;
    }

    public static RouteGroupBuilder MapContactApiV2(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllContacts)
            .DocumentGetRequest<List<ContactRecord>>("GetAllContactsV2", "Gets all the contacts");

        group.MapGet("{id}", GetContact)
            .DocumentGetRequest<ContactRecord>("GetContactV2", "Gets a contact");

        group.MapPost("/", PostContact)
            .AddEndpointFilter<ValidationFilter<ContactBaseRecord>>()
            .DocumentPostRequest<ContactRecord>("PostContactV2", "Creates a contact");

        group.MapPatch("/{id}", PatchContact)
            .AddEndpointFilter<ValidationFilter<ContactBaseRecord>>()
            .DocumentUpdateRequest<ContactRecord>("PatchContactV2", "Updates a contact");

        group.MapDelete("/{id}", DeleteContact)
            .DocumentDeleteRequest("DeleteContactV2", "Deletes a contact");

        return group;
    }

    public static async Task<IResult> GetAllContacts(ContactDataService contactDataService) =>
        Results.Ok(await contactDataService.GetAll());

    public static async Task<IResult> GetContact(ContactDataService contactDataService, int id) =>
        Results.Ok(await contactDataService.Get(id));

    public static async Task<IResult> PostContact(ContactDataService contactDataService, IValidator<ContactBaseRecord> validator, ContactBaseRecord contact) {
        ContactRecord result = await contactDataService.Create(contact);
        return Results.Created($"/contact/{result.Id}", result);
    }

    public static async Task<IResult> PatchContact(ContactDataService contactDataService, IValidator<ContactBaseRecord> validator, ContactBaseRecord contact, int id) =>
        Results.Ok(await contactDataService.Patch(contact, id));

    public static async Task<IResult> PutContact(ContactDataService contactDataService, IValidator<ContactBaseRecord> validator, ContactBaseRecord contact, int id) =>
        Results.Ok(await contactDataService.Update(contact, id));

    public static async Task<IResult> DeleteContact(ContactDataService contactDataService, int id)
    {
        await contactDataService.Delete(id);
        return Results.NoContent();
    }
}
