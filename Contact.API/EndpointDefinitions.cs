namespace Contact.API;

public static class EndpointDefinitions
{
    public static WebApplication AddEndpoints(this WebApplication app)
    {
        app.MapGroup("/contact")
           .MapContactApi()
           .WithTags("Contact");

        app.MapGroup("/error")
           .MapErrorApi()
           .WithTags("Error");

        return app;
    }
}
