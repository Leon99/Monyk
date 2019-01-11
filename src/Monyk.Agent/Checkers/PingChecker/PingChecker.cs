using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Monyk.Agent.Checkers.PingChecker
{
    public class PingChecker : IChecker<PingCheckConfig>
    {
        private readonly IPing _ping;

        public PingChecker(IPing ping)
        {
            _ping = ping;
        }

        public async Task<CheckResult> RunCheckAsync(PingCheckConfig config)
        {
            var result = await _ping.SendAsync(config.Host);
            return new CheckResult
            {
                Status = result.Status == IPStatus.Success ? CheckResultStatus.Success : CheckResultStatus.Failure
            };
        }
    }
}
