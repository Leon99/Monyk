using System.Threading.Tasks;

namespace Monyk.Agent.Checkers
{
    public interface IChecker<in TConfig>
    {
        Task<CheckResult> RunCheckAsync(TConfig config);
    }
}
