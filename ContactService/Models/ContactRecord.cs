namespace ContactService.Models
{
    [SwaggerSchemaFilter(typeof(SwaggerExample))]
    public record ContactRecord(
        int Id,
        string Name
    );

    public static class ContactRecordExample
    {
        public static readonly OpenApiObject Example = new()
        {
            ["Id"] = new OpenApiInteger(3),
            ["Name"] = new OpenApiString("An awesome product")
        };
    }
}
