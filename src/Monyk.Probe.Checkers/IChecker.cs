using System.Threading.Tasks;

namespace Monyk.Probe.Checkers
{
    public interface IChecker<in TConfig>
    {
        Task<CheckResult> RunCheckAsync(TConfig config);
    }
}
