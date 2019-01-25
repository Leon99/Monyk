using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monyk.Common.Communicator;
using Monyk.Common.Models;
using Monyk.Probe.Checkers;

namespace Monyk.Probe.Main
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureLogging(builder => { builder.AddConsole(); })
                .ConfigureServices(services =>
                {
                    services.AddHttpClient();

                    services.AddHostedService<BootstrapService>();

                    services.AddSingleton<IReceiver<CheckRequest>, Transceiver<CheckRequest>>();
                    services.AddSingleton<ITransmitter<CheckResult>, Transceiver<CheckResult>>();

                    services.AddSingleton<CheckerFactory>();
                    services.AddTransient<IPing, Ping>();
                    services.AddTransient<IChecker, PingChecker>();
                    services.AddSingleton<IChecker, HttpChecker>();
                })
                .ConfigureHostConfiguration(builder => { builder.AddEnvironmentVariables(); })
                .Build()
                .RunAsync();
        }
    }
}
