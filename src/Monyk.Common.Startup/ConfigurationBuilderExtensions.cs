using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Monyk.Common.Startup
{
    public static class ConfigurationBuilderExtensions
    {
        public static void AddUserSecretsForMainAssembly(this IConfigurationBuilder config, IHostingEnvironment hostingEnvironment)
        {
            var assembly = Assembly.Load(new AssemblyName(hostingEnvironment.ApplicationName));
            if (assembly != null)
                config.AddUserSecrets(assembly, true);
        }
    }
}