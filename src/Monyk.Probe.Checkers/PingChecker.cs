using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Monyk.Common.Models;

namespace Monyk.Probe.Checkers
{
    public class PingChecker : IChecker
    {
        private readonly IPingFactory _pingFactory;
        private readonly ILogger<PingChecker> _logger;

        public PingChecker(IPingFactory pingFactory, ILogger<PingChecker> logger)
        {
            _pingFactory = pingFactory;
            _logger = logger;
        }

        public async Task<CheckResult> RunCheckAsync(CheckConfiguration config)
        {
            var ping = _pingFactory.Create();
            try
            {
                var result = await ping.SendAsync(config.Target);
                return new CheckResult
                {
                    Status = result.Status == IPStatus.Success ? CheckResultStatus.Success : CheckResultStatus.Failure
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null);
                return new CheckResult
                {
                    Status = CheckResultStatus.Failure, Description = ex.Message
                };
            }
        }
    }
}
