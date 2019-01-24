using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Monyk.Common.Models;

namespace Monyk.Probe.Checkers
{
    public class PingChecker : IChecker
    {
        private readonly IPing _ping;

        public PingChecker(IPing ping)
        {
            _ping = ping;
        }

        public async Task<CheckResult> RunCheckAsync(CheckConfiguration config)
        {
            var result = await _ping.SendAsync(config.Target);
            return new CheckResult
            {
                Status = result.Status == IPStatus.Success ? CheckResultStatus.Success : CheckResultStatus.Failure
            };
        }
    }
}
