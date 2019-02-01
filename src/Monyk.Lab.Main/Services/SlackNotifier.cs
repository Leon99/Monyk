using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Monyk.Common.Models;
using Monyk.GroundControl.ApiClient;

namespace Monyk.Lab.Main.Services
{
    public class SlackNotifierSettings
    {
        public IEnumerable<string> WebHooks { get; set; }
    }

    public class SlackNotifier : IResultProcessor
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly SlackNotifierSettings _settings;
        private readonly IGroundControlApi _gcApi;
        private static readonly JsonMediaTypeFormatter Formatter = new JsonMediaTypeFormatter();

        public SlackNotifier(IHttpClientFactory httpClientFactory, SlackNotifierSettings settings, IGroundControlApi gcApi)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings;
            _gcApi = gcApi;
        }

        public async Task RunAsync(CheckResult result)
        {
            var httpClient = _httpClientFactory.CreateClient();
            if (result.Status != CheckResultStatus.Success)
            {
                var monitor = await _gcApi.GetMonitorAsync(result.MonitorId);
                foreach (var webHook in _settings.WebHooks)
                {
                    await httpClient.PostAsync(webHook, new {text = $"{monitor.Type} check on {monitor.Target} resulted in *{result.Status}*"}, Formatter);
                }
            }
        }
    }
}
