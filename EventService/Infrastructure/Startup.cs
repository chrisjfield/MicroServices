namespace EventService.Infrastructure
{
    public static class Startup
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(new EventDataService());
        }

        public static void AddEndpoints(this WebApplication app)
        {
            app.MapGet("/event", (EventDataService eventDataService, int id) => eventDataService.GetEvent(id))
               .DocumentGetRequest<EventRecord>("Events", "GetEvents", "Gets an event");
        }
    }
}
