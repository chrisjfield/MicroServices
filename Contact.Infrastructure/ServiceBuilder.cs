using Contact.Infrastructure.Services;
using Microsoft.OpenApi.Models;

namespace Contact.Infrastructure;

public static class ServiceBuilder
{
    private readonly static string AllowAllCorsPolicy = "AllowAll";

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.AddDatabase();
        builder.AddDataSerivces();
        builder.AddValidation();
        builder.AddSwagger();
        builder.AddRateLimiting();
        builder.AddCorsPolicies();
    }

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

    private static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ContactDb>(options => options.UseInMemoryDatabase("applicationDb"));
    }

    private static void AddDataSerivces(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ContactDataService>();
    }

    private static void AddSwagger(this WebApplicationBuilder builder)
    {
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

    private static void AddValidation(this WebApplicationBuilder builder)
    {

        builder.Services.AddValidatorsFromAssemblyContaining<ContactRecord>();
    }

    private static void AddRateLimiting(this WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        builder.Services.AddInMemoryRateLimiting();
        builder.Services.Configure<ClientRateLimitOptions>(builder.Configuration.GetSection("ClientRateLimiting"));
    }

    private static void AddCorsPolicies(this WebApplicationBuilder builder)
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
}
