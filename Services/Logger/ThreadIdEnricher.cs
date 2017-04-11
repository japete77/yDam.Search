using Serilog.Core;
using Serilog.Events;

namespace yDevs.Services.Logger
{
    public class ThreadIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                    "ThreadId", System.Environment.CurrentManagedThreadId));
        }
    }
}