using System.Net.Http;
using System.Threading.Tasks;

namespace Monyk.Agent.Checkers.HttpChecker
{
    public class HttpChecker : IChecker<HttpCheckConfig>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpChecker(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CheckResult> RunCheckAsync(HttpCheckConfig config)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(config.Url);
            return new CheckResult
            {
                Status = response.IsSuccessStatusCode ? CheckResultStatus.Success : CheckResultStatus.Failure
            };
        }
    }
}
