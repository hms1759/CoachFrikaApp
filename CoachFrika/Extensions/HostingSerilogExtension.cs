using Serilog;
using Serilog.Events;

namespace CoachFrika.Extensions
{
    public static class HostingSerilogExtension
    {
        public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
        {
            // Set up Serilog with Sentry as a log sink
            ConfigureSerilogLogging(builder);

            // Configure Sentry for error tracking
            ConfigureSentry(builder);

            return builder;
        }

        private static void ConfigureSerilogLogging(WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                var sentryDsn = hostingContext.Configuration["Sentry:Dsn"]; // You can load this from appsettings.json or environment variables

                loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .Enrich.FromLogContext()
                    .WriteTo.File(
                        path: @"logs\log.txt",
                        rollingInterval: RollingInterval.Day,
                        restrictedToMinimumLevel: LogEventLevel.Information,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                        shared: true
                    );
                  //.WriteTo.Sentry(o =>
                  //{
                  //    o.Dsn = sentryDsn;
                  //    o.MinimumBreadcrumbLevel = LogEventLevel.Debug;
                  //    o.MinimumEventLevel = LogEventLevel.Error;
                  //});
            });
        }

        private static void ConfigureSentry(WebApplicationBuilder builder)
        {
            builder.WebHost.UseSentry(options =>
            {
                options.Dsn = "https://899540a3da044867920923cba43ce1e1@o1009331.ingest.us.sentry.io/5973444";
                options.Debug = true; // Enable debug mode for initial configuration
            });
        }

    }
}
