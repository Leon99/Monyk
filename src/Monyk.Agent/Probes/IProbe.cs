using System.Threading.Tasks;

namespace Monyk.Agent.Probes
{
    public interface IProbe<in TConfig>
    {
        Task<VerificationResult> RunVerificationAsync(TConfig probeConfig);
    }
}
