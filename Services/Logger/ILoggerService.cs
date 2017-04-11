using Serilog;

namespace yDevs.Services.Logger
{
    public interface ILoggerService
    {
        ILogger Logger();
    }
}