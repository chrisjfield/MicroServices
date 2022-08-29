using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace Contact.Infrastructure.Telemetry;
public class OTelEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var activity = Activity.Current;

        if (activity != null)
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("trace.id", activity.TraceId));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("span.id", activity.SpanId));
        }
    }
}