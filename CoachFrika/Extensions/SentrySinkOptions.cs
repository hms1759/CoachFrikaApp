using Serilog.Events;

namespace CoachFrika.Extensions
{
    internal class SentrySinkOptions
    {
        public string Dsn { get; set; }
        public LogEventLevel MinimumBreadcrumbLevel { get; set; }
        public LogEventLevel MinimumEventLevel { get; set; }
    }
}