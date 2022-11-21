using Swashbuckle.AspNetCore.Annotations;

namespace Contact.Infrastructure;

public static class RouteHandlerExtensions
{
    public static void DocumentGetRequest<T>(this RouteHandlerBuilder builder, string name, string description)
    {
        builder
           .DocumentBaseRequest(name, description)
           .Produces<T>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status404NotFound);
    }

    public static void DocumentPostRequest<T>(this RouteHandlerBuilder builder, string name, string description)
    {
        builder
           .DocumentBaseRequest(name, description)
           .Produces<T>(StatusCodes.Status201Created)
           .Produces<ValidationErrors>(StatusCodes.Status400BadRequest);
    }

    public static void DocumentPutRequest<T>(this RouteHandlerBuilder builder, string name, string description)
    {
        builder
           .DocumentBaseRequest(name, description)
           .Produces<T>(StatusCodes.Status200OK)
           .Produces<ValidationErrors>(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status404NotFound);
    }

    public static void DocumentDeleteRequest(this RouteHandlerBuilder builder, string name, string description)
    {
        builder
           .DocumentBaseRequest(name, description)
           .Produces(StatusCodes.Status204NoContent)
           .Produces(StatusCodes.Status404NotFound);
    }

    private static RouteHandlerBuilder DocumentBaseRequest(this RouteHandlerBuilder builder, string name, string description)
    {
        return builder
           .WithName(name)
           .WithMetadata(new SwaggerOperationAttribute(description))
           .Produces(StatusCodes.Status429TooManyRequests)
           .Produces(StatusCodes.Status401Unauthorized);
    }
}
