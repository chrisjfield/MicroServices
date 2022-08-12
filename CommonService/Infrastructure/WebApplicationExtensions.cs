using Microsoft.Extensions.DependencyInjection;
using CommonService.Infrastructure;

namespace CommonService
{
    public static class WebApplicationExtensions
    {
        public readonly static string AllowAllCorsPolicy = "AllowAll";

        public static void AddSwagger(this WebApplicationBuilder builder)
        {
            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen((options) => options.EnableAnnotations());
        }

        public static void AddValidation<T>(this WebApplicationBuilder builder)
        {
            
            builder.Services.AddValidatorsFromAssemblyContaining<T>();
        }

        public static void AddCorsPolicies(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: AllowAllCorsPolicy, builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    }
                );
            });
        }

        public static void AddDocumentation(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        public static void RegisterMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }        
    }
}