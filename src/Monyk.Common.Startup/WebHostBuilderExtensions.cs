using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

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
    }
}
