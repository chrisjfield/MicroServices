namespace CommonService;

public static class ValidationHelper
{
    public static ValidationErrors? ValidateRecord<T>(this IValidator<T> validator, T objectToValidate)
    {
        List<string> errors = new();
        var validationResult = validator.Validate(objectToValidate);
        if (!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(e => errors.Add(e.ErrorMessage));
        }

        return errors.Count > 0 ? new ValidationErrors(errors) : null;
    }
}
