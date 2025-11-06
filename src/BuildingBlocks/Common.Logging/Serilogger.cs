using Microsoft.Extensions.Hosting;
using Serilog;

namespace Common.Logging;

public static class Serilogger
{
    public static Action<HostBuilderContext, LoggerConfiguration> Configure => (context, configuration) =>
    {
        var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
        var environmentName = context.HostingEnvironment.EnvironmentName ?? "Development";
        configuration.WriteTo.Debug()
            .WriteTo.Console(
                outputTemplate:
                "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] [{SourceContext}] {Message}{NewLine}{Exception}")
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Environment", environmentName)
            .Enrich.WithProperty("ApplicationName", applicationName)
            .ReadFrom.Configuration(context.Configuration);
    };
}