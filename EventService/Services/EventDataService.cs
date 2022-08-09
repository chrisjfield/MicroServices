namespace EventService.Services
{
    public class EventDataService
    {
        private readonly EventDb EventDb;
        private readonly IValidator<EventRecord> validator;

        public EventDataService(EventDb EventDb, IValidator<EventRecord> validator)
        {
            this.EventDb = EventDb;
            this.validator = validator;
        }

        public async Task<IResult> GetAllEvents()
        {
            List<EventRecord> Events = await EventDb.Events.ToListAsync();
            return Results.Ok(Events);
        }

        public async Task<IResult> GetEvent(int id)
        {
            EventRecord? Event = await EventDb.Events.FindAsync(id);

            if (Event == null) { return Results.NotFound(); }
            return Results.Ok(Event);
        }

        public async Task<IResult> PostEvent(EventRecord Event)
        {
            ValidationErrors? modelValidation = validator.ValidateRecord(Event);
            if (modelValidation != null) { return Results.BadRequest(modelValidation); }

            await EventDb.Events.AddAsync(Event);
            await EventDb.SaveChangesAsync();
            return Results.Created($"/Event/{Event.Id}", Event);
        }

        public async Task<IResult> PutEvent(EventRecord updateEvent, int id)
        {
            if (updateEvent.Id != id) return Results.BadRequest(new ValidationErrors(new() { "'Id' must match the Id value in the request body" }));

            ValidationErrors? modelValidation = validator.ValidateRecord(updateEvent);
            if (modelValidation != null) { return Results.BadRequest(modelValidation); }

            EventRecord? Event = await EventDb.Events.AsNoTracking().FirstOrDefaultAsync((c) => c.Id == id);
            if (Event is null) return Results.NotFound();

            EventDb.Events.Update(updateEvent);
            await EventDb.SaveChangesAsync();
            return Results.Ok(updateEvent);
        }

        public async Task<IResult> DeleteEvent(int id)
        {
            EventRecord? Event = await EventDb.Events.FindAsync(id);
            if (Event is null) return Results.NotFound();

            EventDb.Events.Remove(Event);
            await EventDb.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}
