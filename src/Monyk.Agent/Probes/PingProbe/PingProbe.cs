using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Monyk.Agent.Probes.PingProbe
{
    public class PingProbe : IProbe<PingProbeConfig>
    {
        private readonly IPing _ping;

        public PingProbe(IPing ping)
        {
            _ping = ping;
        }

        public async Task<VerificationResult> RunVerificationAsync(PingProbeConfig probeConfig)
        {
            var result = await _ping.SendAsync(probeConfig.Host);
            return new VerificationResult
            {
                Status = result.Status == IPStatus.Success ? VerificationResultStatus.Success : VerificationResultStatus.Failure
            };
        }
    }
}
