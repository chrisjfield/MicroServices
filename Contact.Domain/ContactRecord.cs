namespace Contact.Domain;

[SwaggerSchemaFilter(typeof(SwaggerExample))]
public record ContactRecord(
    int Id,
    string Name,
    string? Gender = null,
    DateTime? DateOfBirth = null
) : ContactBaseRecord(Name, Gender, DateOfBirth);

public static class ContactRecordExample
{
    public static readonly OpenApiObject Example = new()
    {
        ["Id"] = new OpenApiInteger(3),
        ["Name"] = new OpenApiString("Chris Field"),
        ["Gender"] = new OpenApiString("Male"),
        ["DateOfBirth"] = new OpenApiDate(new DateTime(1992, 11, 21))
    };
}
