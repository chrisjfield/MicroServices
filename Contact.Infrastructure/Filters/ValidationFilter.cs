namespace Contact.Infrastructure.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    private readonly ILogger<ValidationFilter<T>> _logger;

    public ValidationFilter(ILoggerFactory _loggerFactory)
    {
        _logger = _loggerFactory.CreateLogger<ValidationFilter<T>>();
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        IValidator<T> validator = context.GetArgument<IValidator<T>>(1);
        T objectToValidate = context.GetArgument<T>(2);

        var result = await validator.ValidateAsync(objectToValidate);
        if (!result.IsValid)
        {
            ValidationErrors errors = result.Errors.GetErrors();
            _logger.LogInformation($"Validation Failed: {string.Join("; ", errors.Validation)}");

            return Results.BadRequest(errors);
        }

        return await next(context);
    }
}
