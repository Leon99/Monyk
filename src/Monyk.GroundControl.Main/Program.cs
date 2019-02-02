using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Monyk.Common.Startup;

namespace Monyk.GroundControl.Main
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost
                    .CreateDefaultBuilder<Startup>(args)
                    .UseAppConfigurationWithYaml(args)
                ;
        }
    }
}
