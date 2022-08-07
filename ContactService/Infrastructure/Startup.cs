namespace ContactService.Infrastructure
{
    public static class Startup
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(new ContactDataService());
        }

        public static void AddEndpoints(this WebApplication app)
        {
            app.MapGet("/contact", GetContact)
               .DocumentGetRequest<ContactRecord>("Contacts", "GetContact", "Gets a contact");
        }

        private readonly static Delegate GetContact = (ContactDataService contactDataService, int id) => contactDataService.GetContact(id);
    }
}
