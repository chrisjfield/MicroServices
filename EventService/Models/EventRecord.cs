namespace EventService.Models
{
    [SwaggerSchemaFilter(typeof(SwaggerExample))]
    public record EventRecord(
        int Id,
        string Name
    );

    public static class EventRecordExample
    {
        public static readonly OpenApiObject Example = new()
        {
            ["Id"] = new OpenApiInteger(3),
            ["Name"] = new OpenApiString("An awesome event")
        };
    }
}
