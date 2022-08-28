namespace Contact.Infrastructure;

public static class ApplicationBuilder
{
    public static void AddDocumentation(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    public static void RegisterMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseMiddleware<AuthenticationMiddleware>();
    }

    public static void UseRateLimiting(this WebApplication app)
    {
        app.UseClientRateLimiting();
    }
}
