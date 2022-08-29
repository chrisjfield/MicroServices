using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Enrichers.Span;

namespace Contact.Infrastructure.Telemetry;

public static class LoggerBuilder
{
    public static WebApplicationBuilder AddLogger(this WebApplicationBuilder builder)
    {
        builder.Services
                .AddLogging(logBuilder =>
                {
                    string serviceName = builder.Environment.ApplicationName + "." + builder.Environment.EnvironmentName;

                    var loggerConfig = new LoggerConfiguration()
                        .ReadFrom.Configuration(builder.Configuration)
                        .Enrich.With<OTelEnricher>()
                        .Enrich.WithProperty("service.name", serviceName)
                        .Enrich.WithSpan();

                    var newRelicLogUrl = builder.Configuration.GetValue<string>("NewRelic:LogURL");
                    var newRelicApiKey = builder.Configuration.GetValue<string>("NewRelic:ApiKey");
                    if (//!builder.Environment.IsDevelopment() &&
                        !string.IsNullOrEmpty(newRelicLogUrl) &&
                        !string.IsNullOrEmpty(newRelicApiKey))
                    {
                        loggerConfig.WriteTo.NewRelicLogs(
                            endpointUrl: newRelicLogUrl,
                            serviceName,
                            insertKey: newRelicApiKey,
                            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug);
                    }

                    Log.Logger = loggerConfig.CreateLogger();

                    logBuilder
                        //.ClearProviders()       // Clears default console provider.
                        .AddSerilog();
                });

        return builder;
    }
}