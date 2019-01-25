using System.Net.Http;
using System.Threading.Tasks;
using Monyk.Common.Models;

namespace Monyk.Probe.Checkers
{
    public class HttpChecker : IChecker
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpChecker(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CheckResult> RunCheckAsync(CheckConfiguration config)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(config.Target);
            return new CheckResult
            {
                Status = response.IsSuccessStatusCode ? CheckResultStatus.Success : CheckResultStatus.Failure,
            };
        }
    }
}
