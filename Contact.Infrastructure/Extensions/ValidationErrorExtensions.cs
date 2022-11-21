namespace Contact.Infrastructure.Extensions;

public static class ValidationErrorExtensions
{
    public static ValidationErrors GetErrors(this List<ValidationFailure> errors)
    {
        List<string> errorsList = new();
        errors.ForEach(e => errorsList.Add(e.ErrorMessage));

        return new ValidationErrors(errorsList);
    }
}

