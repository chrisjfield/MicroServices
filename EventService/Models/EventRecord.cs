

namespace EventService.Models
{
    [SwaggerSchemaFilter(typeof(SwaggerExample))]
    public record EventRecord(
        int Id,
        string Name,
        string Location,
        DateTime? StartTime
    );

    public static class EventRecordExample
    {
        public static readonly OpenApiObject Example = new()
        {
            ["Id"] = new OpenApiInteger(3),
            ["Name"] = new OpenApiString("My Epic Event"),
            ["Location"] = new OpenApiString("Loughborough HQ"),
            ["StartTime"] = new OpenApiDate(DateTime.Today)
        };
    }

    public class EventRecordValidator : AbstractValidator<EventRecord>
    {
        public EventRecordValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Location).NotEmpty();
        }
    }
}
