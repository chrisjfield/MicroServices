using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http;

namespace CommonService
{
    public static class RouteHandlerExtensions
    {
        public static void DocumentGetRequest<T>(this RouteHandlerBuilder builder, string tag, string name, string description)
        {
            builder
               .WithName(name)
               .WithTags(tag)
               .WithMetadata(new SwaggerOperationAttribute(description))
               .Produces<T>(StatusCodes.Status200OK)
               .Produces(StatusCodes.Status404NotFound);
        }
    }
}
