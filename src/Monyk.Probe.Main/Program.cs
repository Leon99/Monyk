using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Monyk.Common.Startup;

namespace Monyk.Probe.Main
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await WebHost
                .CreateDefaultBuilder(args)
                .UseAppSettingsYaml()
                .ConfigureAppConfiguration(config => config.AddUserSecrets(typeof(Program).Assembly))
                .UseStartup<Startup>()
                .Build()
                .RunAsync();
        }
    }
}
