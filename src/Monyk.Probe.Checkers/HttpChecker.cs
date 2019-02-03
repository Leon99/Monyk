using System;
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
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(config.Target);
            }
            catch (Exception ex)
            {
                return new CheckResult
                {
                    Status = CheckResultStatus.Failure,
                    Description = ex.Message
                };
            }

            return new CheckResult
            {
                Status = response.IsSuccessStatusCode ? CheckResultStatus.Success : CheckResultStatus.Failure,
                Description = $"Received status code: {response.StatusCode} {response.ReasonPhrase}"
            };
        }
    }
}
