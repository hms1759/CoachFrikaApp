

using Serilog;
using Serilog.Events;

namespace CoachFrika.Extensions
{
    public static class HostingSerilogExtension
    {
        public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                var logConfig = loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                                .Enrich.FromLogContext()
                                .WriteTo.File(@"logs\log.txt", rollingInterval: RollingInterval.Day,

                                restrictedToMinimumLevel: LogEventLevel.Information,
                                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                                shared: true);


            });
            return builder;
        }
    }
}
