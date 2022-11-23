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
        app.UseSwaggerUI(
    options =>
    {
        var descriptions = ApiVersions.apiVersions;

        // build a swagger endpoint for each discovered API version
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
    }

    private static void RegisterMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseMiddleware<AuthenticationMiddleware>();
    }
}
