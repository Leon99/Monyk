using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Monyk.Common.Startup
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseAppConfigurationWithYaml(this IWebHostBuilder builder, string[] args)
        {
            builder.UseAppSettingsYaml();
            return builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                if (hostingContext.HostingEnvironment.IsDevelopment())
                {
                    config.AddUserSecretsForMainAssembly(hostingContext.HostingEnvironment);
                }

                config.AddEnvironmentVariables("MONYK_");

                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            });
        }

        public static IWebHostBuilder UseAppSettingsYaml(this IWebHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config
                    .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    .AddYamlFile("appsettings.yml", optional: true)
                    .AddYamlFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.yml", optional: true);
            });
        }

        public static IWebHostBuilder ConfigureLogging(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureLogging((context, builder) =>
            {
                var config = context.Configuration.GetSection("Seq");
                if (config != null)
                {
                    builder.AddSeq(config);
                }
            });
        }
    }
}
