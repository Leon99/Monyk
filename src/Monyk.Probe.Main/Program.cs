using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Monyk.Probe.Main
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureLogging(builder =>
                {
                    builder.AddConsole();
                })
                .ConfigureServices(services =>
                {
                    services.AddHttpClient();
                    services.AddHostedService<BootstrapService>();
                })
                .ConfigureHostConfiguration(builder =>
                {
                    builder.AddEnvironmentVariables();
                })
                .Build();

            await host.RunAsync();
        }
    }
}
