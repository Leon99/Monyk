using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Monyk.Common.Startup;

namespace Monyk.Lab.Main
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .UseAppSettingsYaml()
                .ConfigureAppConfiguration(config => config.AddUserSecrets(typeof(Program).Assembly))
                .UseStartup<Startup>();
    }
}
