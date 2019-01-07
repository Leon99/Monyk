using System.Net.Http;

namespace Monyk.Agent.Tests
{
    public class FakeHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpMessageHandler _messageHandler;

        public FakeHttpClientFactory(HttpMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }

        public HttpClient CreateClient(string name)
        {
            var httpClient = new HttpClient(_messageHandler);

            return httpClient;
        }
    }
}
