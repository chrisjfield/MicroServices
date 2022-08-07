namespace ContactService.Services
{
    public class ContactDataService
    {
        public IResult GetContact(int id)
        {
            if (id == 0) { return Results.NotFound(); }

            Log.Information(id.ToString());

            ContactRecord contact = new(id, "test contact");
            return Results.Ok(contact);
        }
    }
}
