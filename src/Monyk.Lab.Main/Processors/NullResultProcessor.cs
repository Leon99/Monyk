using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Monyk.Common.Models;

namespace Monyk.Lab.Main.Processors
{
    public class NullResultProcessor : IResultProcessor
    {
        private readonly ILogger<NullResultProcessor> _logger;

        public NullResultProcessor(ILogger<NullResultProcessor> logger)
        {
            _logger = logger;
        }

        public async Task RunAsync(CheckResult result)
        {
        }
    }
}
