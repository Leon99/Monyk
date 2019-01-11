using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Monyk.Probe.Main
{
    class BootstrapService : IHostedService
    {
        private readonly ILogger<BootstrapService> _logger;

        public BootstrapService(ILogger<BootstrapService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopped");
            return Task.CompletedTask;
        }
    }
}
