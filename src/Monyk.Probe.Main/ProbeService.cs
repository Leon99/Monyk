using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monyk.Common.Communicator;
using Monyk.Common.Models;
using Monyk.Probe.Checkers;

namespace Monyk.Probe.Main
{
    class ProbeService : IHostedService
    {
        private readonly ILogger<ProbeService> _logger;
        private readonly IReceiver<CheckRequest> _receiver;
        private readonly CheckerFactory _checkerFactory;
        private readonly ITransmitter<CheckResult> _transmitter;

        public ProbeService(ILogger<ProbeService> logger, IReceiver<CheckRequest> receiver, ITransmitter<CheckResult> transmitter, CheckerFactory checkerFactory)
        {
            _logger = logger;
            _receiver = receiver;
            _transmitter = transmitter;
            _checkerFactory = checkerFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Launching Probe");
#pragma warning disable 4014
            _receiver.Received += (sender, check) => RunCheckAsync(check);
#pragma warning restore 4014
            _receiver.StartReception();
            return Task.CompletedTask;
        }

        private async Task RunCheckAsync(CheckRequest request)
        {
            if (request.Configuration == null)
            {
                _logger.LogError("Incomplete message received");
                return;
            }
            var checker = _checkerFactory.Create(request.Type);
            try
            {
                _logger.LogInformation("Running check {CheckId} ({MonitorId})", request.CheckId, request.MonitorId);
                var result = await checker.RunCheckAsync(request.Configuration);
                result.CheckId = request.CheckId;
                result.MonitorId = request.MonitorId;
                _transmitter.Transmit(result);
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
