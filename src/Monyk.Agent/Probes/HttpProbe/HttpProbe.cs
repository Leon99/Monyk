using System.Net.Http;
using System.Threading.Tasks;

namespace Monyk.Agent.Probes.HttpProbe
{
    public class HttpProbe : IProbe<HttpProbeConfig>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpProbe(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<VerificationResult> RunVerificationAsync(HttpProbeConfig probeConfig)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(probeConfig.Url);
            return new VerificationResult
            {
                Status = response.IsSuccessStatusCode ? VerificationResultStatus.Success : VerificationResultStatus.Failure
            };
        }
    }
}
