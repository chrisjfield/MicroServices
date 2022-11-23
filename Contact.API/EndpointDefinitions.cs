namespace Contact.API;

public static class EndpointDefinitions
{
    public static WebApplication AddEndpoints(this WebApplication app, ApiVersionSet versionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/contact")
           .MapContactApi()
           .WithTags("Contact")
           .WithApiVersionSet(versionSet)
           .MapToApiVersion(ApiVersions.version1);

        app.MapGroup("api/v{version:apiVersion}/contact")
           .MapContactApiV2()
           .WithTags("Contact")
           .WithApiVersionSet(versionSet)
           .MapToApiVersion(ApiVersions.version2);

        app.MapGroup("api/error")
           .MapErrorApi()
           .WithTags("Error")
           .WithApiVersionSet(versionSet)
           .IsApiVersionNeutral();

        return app;
    }
}
