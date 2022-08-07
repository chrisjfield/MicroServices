namespace EventService.Infrastructure
{
    public class SwaggerExample : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Example = GetExampleOrNullForType(context.Type);
        }

        private static IOpenApiAny? GetExampleOrNullForType(Type type)
        {
            return type.Name switch
            {
                nameof(EventRecord) => EventRecordExample.Example,
                _ => null,
            };
        }
    }
}
