namespace Contact.Infrastructure;

public static class ApplicationBuilder
{
    public static WebApplication ConfigureApp(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.AddDocumentation();
        app.RegisterMiddleware();
        app.UseRateLimiter();
        app.UseHttpLogging();

        return app;
    }

    private static void AddDocumentation(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    private static void RegisterMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseMiddleware<AuthenticationMiddleware>();
    }
}
