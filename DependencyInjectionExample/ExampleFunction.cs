using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DependencyInjectionExample
{
    using DependencyInjectionExample.Config;

    public class ExampleFunction
    {
        private readonly HttpClient _httpClient;
        private readonly string _testUrlFromConfiguration;
        private readonly ExampleSettingsConfig _exampleTestSettings;

        public ExampleFunction(IHttpClientFactory httpClientFactory, IConfiguration config, IOptions<ExampleSettingsConfig> options)
        {
            _httpClient = httpClientFactory.CreateClient();
            _testUrlFromConfiguration = config.GetValue<string>("TestUrl");
            _exampleTestSettings = options.Value;
        }

        [FunctionName(nameof(SimpleRequest))]
        public async Task<IActionResult> SimpleRequest([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("We have a logger!");
            log.LogInformation("The configured testUrl is with IOptions: {testUrl}", _exampleTestSettings.TestUrl);
            log.LogInformation("The configured testUrl is with IConfiguration: {testUrl}", _testUrlFromConfiguration);

            var responseMessage = await _httpClient.GetAsync(_exampleTestSettings.TestUrl);
            responseMessage.EnsureSuccessStatusCode();
            _ = await responseMessage.Content.ReadAsStringAsync();

            return new OkResult();
        }
    }
}
