using System.Threading.Tasks;
using Monyk.Common.Models;

namespace Monyk.Lab.Main
{
    public interface IResultProcessor
    {
        Task RunAsync(CheckResult result);
    }
}
