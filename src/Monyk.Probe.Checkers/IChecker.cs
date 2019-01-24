using System.Threading.Tasks;
using Monyk.Common.Models;

namespace Monyk.Probe.Checkers
{
    public interface IChecker
    {
        Task<CheckResult> RunCheckAsync(CheckConfiguration config);
    }
}
