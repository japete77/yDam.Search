using Serilog.Core;
using Serilog.Events;

namespace yDevs.Services.Logger
{
    public class MachineNameEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                    "MachineName", System.Environment.MachineName));
        }
    }
}