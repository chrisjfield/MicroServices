namespace EventService.Services
{
    public class EventDataService
    {
        public IResult GetEvent(int id)
        {
            if (id == 0) { return Results.NotFound(); }

            Log.Error(id.ToString());

            EventRecord eventRecord = new(id, "test event");
            return Results.Ok(eventRecord);
        }
    }
}
