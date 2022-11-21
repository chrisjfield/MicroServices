namespace Contact.Infrastructure;

public static class ServiceBuilder
{
    private readonly static string AllowAllCorsPolicy = "AllowAll";
    public readonly static string ApiKeyHeader = "X-ClientId";

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.AddDatabase();
        builder.AddDataSerivces();
        builder.AddValidation();
        builder.AddSwagger();
        builder.AddCorsPolicies();
        builder.AddRateLimitingPolicies();
        builder.AddLogger();
        builder.AddOpenTelemetryInstrumenter();

        return builder;
    }

    private static readonly OpenApiSecurityScheme clientId = new()
    {
        Description = "ClientId must be specified",
        Type = SecuritySchemeType.ApiKey,
        Name = ApiKeyHeader,
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = ApiKeyHeader
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
            options.AddSecurityDefinition(ApiKeyHeader, clientId);
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

    private static void AddRateLimitingPolicies(this WebApplicationBuilder builder)
    {
        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                httpContext.Request.Headers.TryGetValue(ApiKeyHeader, out var clientId);

                return RateLimitPartition.GetSlidingWindowLimiter(
                    partitionKey: clientId.ToString(),
                    factory: partition => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(1),
                        SegmentsPerWindow = 10,
                        QueueLimit = 0,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    }
                );
            });
        });
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
            });
        });
    }
}
