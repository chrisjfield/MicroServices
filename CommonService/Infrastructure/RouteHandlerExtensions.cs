using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http;

namespace CommonService
{
    public static class RouteHandlerExtensions
    {
        public static void DocumentGetRequest<T>(this RouteHandlerBuilder builder, string tag, string name, string description)
        {
            builder
               .DocumentBaseRequest(tag, name, description)
               .Produces<T>(StatusCodes.Status200OK)
               .Produces(StatusCodes.Status404NotFound);
        }

        public static void DocumentPostRequest<T>(this RouteHandlerBuilder builder, string tag, string name, string description)
        {
            builder
               .DocumentBaseRequest(tag, name, description)
               .Produces<T>(StatusCodes.Status201Created)
               .Produces<ValidationErrors>(StatusCodes.Status400BadRequest);
        }

        public static void DocumentPutRequest<T>(this RouteHandlerBuilder builder, string tag, string name, string description)
        {
            builder
               .DocumentBaseRequest(tag, name, description)
               .Produces<T>(StatusCodes.Status200OK)
               .Produces<ValidationErrors>(StatusCodes.Status400BadRequest)
               .Produces(StatusCodes.Status404NotFound);
        }

        public static void DocumentDeleteRequest(this RouteHandlerBuilder builder, string tag, string name, string description)
        {
            builder
               .DocumentBaseRequest(tag, name, description)
               .Produces(StatusCodes.Status204NoContent)
               .Produces(StatusCodes.Status404NotFound);
        }

        private static RouteHandlerBuilder DocumentBaseRequest(this RouteHandlerBuilder builder, string tag, string name, string description)
        {
            return builder
               .WithName(name)
               .WithTags(tag)
               .WithMetadata(new SwaggerOperationAttribute(description));
        }
    }
}
