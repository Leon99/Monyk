using System.Threading.Tasks;
using Monyk.Common.Models;

namespace Monyk.Lab.Main.Processors
{
    public class NullResultProcessor : IResultProcessor
    {
        public async Task RunAsync(CheckResult result)
        {
        }
    }
}
