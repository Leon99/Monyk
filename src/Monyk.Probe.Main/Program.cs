using Microsoft.AspNetCore.Hosting;
using Monyk.Common.Startup;

namespace Monyk.Probe.Main
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
                    .CreateBuilder(args)
                    .UseStartup<Startup>()
                ;
        }
    }
}
