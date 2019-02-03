using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Monyk.Common.Models;

namespace Monyk.Probe.Checkers
{
    public class PingChecker : IChecker
    {
        private readonly ILogger<PingChecker> _logger;
        private readonly IPingFactory _pingFactory;

        public PingChecker(ILogger<PingChecker> logger, IPingFactory pingFactory)
        {
            _logger = logger;
            _pingFactory = pingFactory;
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
                    Status = CheckResultStatus.Failure, Description = ex.InnerException?.Message ?? ex.Message
                };
            }
        }
    }
}
