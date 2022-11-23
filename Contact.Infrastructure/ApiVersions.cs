namespace Contact.Infrastructure;

public static class ApiVersions
{
    public readonly static ApiVersion version1 = new(1);
    public readonly static ApiVersion version2 = new(2);

    // bit of a hack as Swagger did not seem to want to read the versions properly
    // hopefully fixed in future versions (then this can sit in API project)
    public readonly static ApiVersionDescription[] apiVersions = {
        new(version1, "v1", true),
        new(version2, "v2", false)
    };

    public static ApiVersionSet ApiVersionSet(this WebApplication app) => app
        .NewApiVersionSet()
        .HasApiVersion(version2)
        .HasDeprecatedApiVersion(version1)
        .Build();
}