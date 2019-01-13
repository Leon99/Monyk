using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monyk.Common.Communicator.Models;
using Monyk.Common.Communicator.Services;
using Monyk.Probe.Checkers.HttpChecker;
using Monyk.Probe.Checkers.PingChecker;

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
                    services.AddSingleton<IReceiver<CheckRequest>, Transceiver<CheckRequest>>();
                    
                    services.AddSingleton<PingChecker>();
                    services.AddSingleton<HttpChecker>();
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
