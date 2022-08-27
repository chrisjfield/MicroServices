using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.Extensions.Configuration;
using OpenTelemetry;
using System.Diagnostics;

namespace CommonService
{
    public static class OpenTelemetryBuilder
    {
        public static void OpenTelemetryInstrumenter(this WebApplicationBuilder builder)
        {            
            builder.Services
            .AddOpenTelemetryTracing(traceBuilder =>
            {
                traceBuilder.Configure((serviceProvider, traceBuilder) =>
                {
                    // Make the logger factory available to the dependency injection
                    // container so that it may be injected into the OpenTelemetry Tracer.
                    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                });

                traceBuilder
                    .SetResourceBuilder(ResourceBuilder
                        .CreateDefault()
                        .AddService(builder.Environment.ApplicationName + "." + builder.Environment.EnvironmentName)
                        .AddAttributes(new Dictionary<string, object> {
                            { "environment", builder.Environment.EnvironmentName }
                        })
                        .AddTelemetrySdk()) // required for 'transactions' link in New Relic when using the OpenTelemetry Collector
                    .AddSource("ChrisFExampleApi", "Access.Logs")
                    .AddConsoleExporter()
                    // Captures incoming HTTP requests
                    // https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/src/OpenTelemetry.Instrumentation.AspNetCore
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true; // Capture errors.
                        options.Filter = message =>
                            message is not null &&
                            !UriHelper.GetEncodedUrl(message.Request).Contains("swagger"); // Filter out swagger docs.
                    })
                    // Captures outgoing HTTP requests through System.Net.Http.HttpClient
                    // https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/src/OpenTelemetry.Instrumentation.Http
                    .AddHttpClientInstrumentation(options =>
                    {
                        options.Filter = message =>
                            message is not null &&
                            message.RequestUri is not null &&
                            !message.RequestUri.Host.Contains("visualstudio") &&
                            !message.RequestUri.Host.Contains("newrelic"); // Filter out newrelic logs.
                    })
                    // Sample selects what data from the SPANS collected will be sampled.
                    // https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/sdk.md#sampler
                    .SetSampler(new AlwaysOnSampler());

                // Use the OTLP Exporter to send span directly to New Relic when you do not want to use a collector.
                var newRelicTraceUrl = builder.Configuration.GetValue<string>("NewRelic:TraceUrl");
                var newRelicApiKey = builder.Configuration.GetValue<string>("NewRelic:ApiKey");
                var newRelicEndpoint = builder.Configuration.GetValue<string>("NewRelic:Endpoint");
                if (!string.IsNullOrEmpty(newRelicTraceUrl) &&
                    !string.IsNullOrEmpty(newRelicApiKey) &&
                    !string.IsNullOrEmpty(newRelicEndpoint))
                {
                    traceBuilder.AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(newRelicEndpoint);
                        options.Headers = "api-key = " + newRelicApiKey;
                        options.ExportProcessorType = ExportProcessorType.Batch;
                        options.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>
                        {
                            // Set to Default values from abstract class BatchExportProcessor
                            ExporterTimeoutMilliseconds = 30000,
                            MaxExportBatchSize = 512,
                            MaxQueueSize = 2048,
                            ScheduledDelayMilliseconds = 5000
                        };
                    });
                };
            });
        }
    }
}
