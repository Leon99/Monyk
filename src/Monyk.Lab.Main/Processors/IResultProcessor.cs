using System.Threading.Tasks;
using Monyk.Common.Models;

namespace Monyk.Lab.Main.Processors
{
    public interface IResultProcessor
    {
        Task RunAsync(CheckResult result);
    }
}
