using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monyk.Common.Communicator.Models;
using Monyk.Common.Communicator.Services;

namespace Monyk.Probe.Main
{
    class BootstrapService : IHostedService
    {
        private readonly ILogger<BootstrapService> _logger;
        private readonly IReceiver<CheckRequest> _receiver;

        public BootstrapService(ILogger<BootstrapService> logger, IReceiver<CheckRequest> receiver)
        {
            _logger = logger;
            _receiver = receiver;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Launching the probe");
            _receiver.Received += (sender, check) => RunCheck(check);
            _receiver.StartReception();
            return Task.CompletedTask;
        }

        private void RunCheck(CheckRequest check)
        {
            _logger.LogInformation($"({check.Type}) {check.Target}");
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Taking down the probe");
            return Task.CompletedTask;
        }
    }
}
