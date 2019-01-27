using System.Collections.Generic;
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
        private readonly IEnumerable<IResultProcessor> _processors;

        public LabService(ILogger<LabService> logger, IReceiver<CheckResult> receiver, IEnumerable<IResultProcessor> processors)
        {
            _logger = logger;
            _receiver = receiver;
            _processors = processors;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Opening the lab");
            _receiver.Received += async (sender, result) => await ProcessResultAsync(result);
            _receiver.StartReception();
            return Task.CompletedTask;
        }

        private async Task ProcessResultAsync(CheckResult result)
        {
            _logger.LogInformation($"Processing result of check {result.CheckId}");
            foreach (var processor in _processors)
            {
                await processor.RunAsync(result);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Shutting down the lab");
            return Task.CompletedTask;
        }
    }
}
