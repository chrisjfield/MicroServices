namespace Contact.Infrastructure;

public static class ApplicationBuilder
{
    public static WebApplication ConfigureApp(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.AddDocumentation();
        app.RegisterMiddleware();
        app.AddRateLimiter();
        app.UseHttpLogging();

        return app;
    }

    private static void AddRateLimiter(this WebApplication app)
    {
        app.UseRateLimiter(new RateLimiterOptions()
        {
            RejectionStatusCode = StatusCodes.Status429TooManyRequests,
            GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                httpContext.Request.Headers.TryGetValue(ServiceBuilder.ApiKeyHeader, out var clientId);

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: clientId.ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        QueueLimit = 0,
                        Window = TimeSpan.FromMinutes(1),
                        AutoReplenishment = true,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    }
                );
            })
        });
    }

    private static void AddDocumentation(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    private static void RegisterMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseMiddleware<AuthenticationMiddleware>();
    }
}
