using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monyk.Common.Communicator.Models;
using Monyk.Common.Communicator.Services;
using Monyk.Probe.Checkers;

namespace Monyk.Probe.Main
{
    class BootstrapService : IHostedService
    {
        private readonly ILogger<BootstrapService> _logger;
        private readonly IReceiver<CheckRequest> _receiver;
        private readonly CheckerFactory _checkerFactory;
        private readonly ITransmitter<CheckResult> _transmitter;

        public BootstrapService(ILogger<BootstrapService> logger, IReceiver<CheckRequest> receiver, ITransmitter<CheckResult> transmitter, CheckerFactory checkerFactory)
        {
            _logger = logger;
            _receiver = receiver;
            _transmitter = transmitter;
            _checkerFactory = checkerFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Launching the probe");
            _receiver.Received += (sender, check) => RunCheckAsync(check);
            _receiver.StartReception();
            return Task.CompletedTask;
        }

        private async Task RunCheckAsync(CheckRequest check)
        {
            if (check.Configuration == null)
            {
                _logger.LogError("Incomplete message received");
                return;
            }
            _logger.LogInformation($"({check.Type}) {check.Configuration.Target}");
            var checker = _checkerFactory.Create(check.Type);
            try
            {
                var results = await checker.RunCheckAsync(check.Configuration);
                _transmitter.Transmit(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during check execution");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Taking down the probe");
            return Task.CompletedTask;
        }
    }
}
