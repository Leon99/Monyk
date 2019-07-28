using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Monyk.Common.Models;
using Monyk.GroundControl.ApiClient;
using Monyk.GroundControl.Models;

namespace Monyk.Lab.Main.Processors
{
    public class WebHookNotifierSettings
    {
        public string Url { get; set; }
    }

    public class WebHookNotifier : IResultProcessor
    {
        private readonly ILogger<WebHookNotifier> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WebHookNotifierSettings _settings;
        private readonly IGroundControlApi _gcApi;
        private static readonly JsonMediaTypeFormatter Formatter = new JsonMediaTypeFormatter();

        public WebHookNotifier(WebHookNotifierSettings settings, ILogger<WebHookNotifier> logger, IHttpClientFactory httpClientFactory, IGroundControlApi gcApi)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _settings = settings;
            _gcApi = gcApi;
        }

        public async Task RunAsync(CheckResult result)
        {
            var httpClient = _httpClientFactory.CreateClient();
            if (result.Status != CheckResultStatus.Success)
            {
                MonitorEntity monitorEntity = null;
                try
                {
                    monitorEntity = await _gcApi.GetMonitorAsync(result.MonitorId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to retrieve monitor details");
                }

                if (monitorEntity != null)
                {
                        await httpClient.PostAsync(_settings.Url, new {text = $"{monitorEntity.Type} check on {monitorEntity.Target} (_{monitorEntity.Description}_) resulted in *{result.Status}*. Details: _{result.Description}_"}, Formatter);
                }
            }
        }
    }
}
