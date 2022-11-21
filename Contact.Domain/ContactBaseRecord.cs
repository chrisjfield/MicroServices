namespace Contact.Domain;

[SwaggerSchemaFilter(typeof(SwaggerExample))]
public record ContactBaseRecord(
    string Name,
    string? Gender = null,
    DateTime? DateOfBirth = null
);

public static class ContactBaseRecordExample
{
    public static readonly OpenApiObject Example = new()
    {
        ["Name"] = new OpenApiString("Chris Field"),
        ["Gender"] = new OpenApiString("Male"),
        ["DateOfBirth"] = new OpenApiDate(new DateTime(1992, 11, 21))
    };
}

public class ContactBaseRecordValidator : AbstractValidator<ContactBaseRecord>
{
    readonly List<string> genders = new() { "Female", "Male", "Other" };

    public ContactBaseRecordValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.DateOfBirth).LessThan(DateTime.Today).WithMessage("Date of birth must be in the past");
        RuleFor(c => c.Gender).Must(g => genders.Contains(g!)).When(c => c.Gender != null).WithMessage("Please only use: " + string.Join(",", genders));
    }
}
