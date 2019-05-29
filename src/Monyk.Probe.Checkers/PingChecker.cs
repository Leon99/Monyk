using System;
using System.Net;
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
                var pingReply = await ping.SendAsync(config.Target);
                var result = new CheckResult
                {
                    Status = pingReply.Status == IPStatus.Success ? CheckResultStatus.Success : CheckResultStatus.Failure,
                };
                if (pingReply.IPAddress != null && !pingReply.IPAddress.Equals(IPAddress.Any))
                {
                    result.Description = $"Resolved IP address: {pingReply.IPAddress}";
                    result.CompletionTime = pingReply.RoundtripTime;
                }
                else
                {
                    result.Description = $"Unable to resolve IP address ({pingReply.Status})";
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new CheckResult
                {
                    Status = CheckResultStatus.Indeterminate, Description = ex.InnerException?.Message ?? ex.Message
                };
            }
        }
    }
}
