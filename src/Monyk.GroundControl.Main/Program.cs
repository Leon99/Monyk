using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Monyk.GroundControl.Main
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            await WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build()
                .RunAsync();
        }
    }
}
