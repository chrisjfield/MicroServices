using Microsoft.Extensions.DependencyInjection;
using CommonService.Infrastructure;
using Microsoft.OpenApi.Models;
using AspNetCoreRateLimit;

namespace CommonService
{
    public static class WebApplicationExtensions
    {
        public readonly static string AllowAllCorsPolicy = "AllowAll";

        private static readonly OpenApiSecurityScheme clientId = new()
        {
            Description = "ClientId must be specified",
            Type = SecuritySchemeType.ApiKey,
            Name = "X-ClientId",
            In = ParameterLocation.Header,
            Scheme = "ApiKeyScheme",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "X-ClientId"
            }
        };

        public static void AddSwagger(this WebApplicationBuilder builder)
        {
            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen((options) =>
            {
                options.EnableAnnotations();
                options.AddSecurityDefinition("X-ClientId", clientId);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { clientId, new List<string>() }
                });
            });
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

        public static void AddRateLimiting(this WebApplicationBuilder builder)
        {
            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            builder.Services.AddInMemoryRateLimiting();
            builder.Services.Configure<ClientRateLimitOptions>(builder.Configuration.GetSection("ClientRateLimiting"));
        }

        public static void UseRateLimiting(this WebApplication app)
        {
            app.UseClientRateLimiting();
        }

        public static void AddDocumentation(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        public static void RegisterMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<AuthenticationMiddleware>();
        }        
    }
}