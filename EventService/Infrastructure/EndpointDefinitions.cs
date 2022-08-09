namespace EventService.Infrastructure
{
    public static class EndpointDefinitions
    {
        public static void AddEndpoints(this WebApplication app)
        {
            app.MapGet("/Event", GetAllEvents)
                .DocumentGetRequest<List<EventRecord>>("Events", "GetAllEvents", "Gets all the Events"); ;

            app.MapGet("/Event/{id}", GetEvent)
                .DocumentGetRequest<EventRecord>("Event", "GetEvent", "Gets a Event");

            app.MapPost("/Event", PostEvent)
                .DocumentPostRequest<EventRecord>("Event", "PostEvent", "Creates a Event");

            app.MapPut("/Event/{id}", PutEvent)
                .DocumentPutRequest<EventRecord>("Event", "PutEvent", "Updates a Event");

            app.MapDelete("/Event/{id}", DeleteEvent)
                .DocumentDeleteRequest("Event", "DeleteEvent", "Deletes a Event");
        }

        internal static async Task<IResult> GetAllEvents(EventDataService EventDataService)
        {
            return await EventDataService.GetAllEvents();
        }

        internal static async Task<IResult> GetEvent(EventDataService EventDataService, int id)
        {
            return await EventDataService.GetEvent(id);
        }

        internal static async Task<IResult> PostEvent(EventDataService EventDataService, EventRecord Event)
        {
            return await EventDataService.PostEvent(Event);
        }

        internal static async Task<IResult> PutEvent(EventDataService EventDataService, EventRecord Event, int id)
        {
            return await EventDataService.PutEvent(Event, id);
        }

        internal static async Task<IResult> DeleteEvent(EventDataService EventDataService, int id)
        {
            return await EventDataService.DeleteEvent(id);
        }
    }
}
