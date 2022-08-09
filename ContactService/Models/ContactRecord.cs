

namespace ContactService.Models
{
    [SwaggerSchemaFilter(typeof(SwaggerExample))]
    public record ContactRecord(
        int Id,
        string Name,
        string? Gender = null,
        DateTime? DateOfBirth = null
    );

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

    public class ContactRecordValidator : AbstractValidator<ContactRecord>
    {
        readonly List<string> genders = new() { "Female", "Male", "Other" };

        public ContactRecordValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.DateOfBirth).LessThan(DateTime.Today).WithMessage("Date of birth must be in the past");
            RuleFor(c => c.Gender).Must(g => genders.Contains(g)).When(c => c.Gender != null).WithMessage("Please only use: " + string.Join(",", genders));
        }
    }
}
