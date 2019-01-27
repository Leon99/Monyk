using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Monyk.Common.Startup
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseAppSettingsYaml(this IWebHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config
                    .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    .AddYamlFile("appsettings.yml", optional: false)
                    .AddYamlFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.yml", optional: false);
            });
        }
    }
}
