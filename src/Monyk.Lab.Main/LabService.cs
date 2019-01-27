using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monyk.Common.Communicator;
using Monyk.Common.Models;

namespace Monyk.Lab.Main
{
    public class LabService : IHostedService
    {
        private readonly ILogger<LabService> _logger;
        private readonly IReceiver<CheckResult> _receiver;

        public LabService(ILogger<LabService> logger, IReceiver<CheckResult> receiver)
        {
            _logger = logger;
            _receiver = receiver;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Opening the lab");
            _receiver.Received += (sender, result) => ProcessResult(result);
            _receiver.StartReception();
            return Task.CompletedTask;
        }

        private void ProcessResult(CheckResult result)
        {
            _logger.LogInformation("Processing results");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Shutting down the lab");
            return Task.CompletedTask;
        }
    }
}
