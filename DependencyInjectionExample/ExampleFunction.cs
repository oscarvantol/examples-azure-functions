using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using DependencyInjectionExample.Config;

namespace DependencyInjectionExample
{
    public class ExampleFunction
    {
        private readonly HttpClient _httpClient;
        private readonly string _testUrl;
        private readonly ExampleSettingsConfig _exampleTestSettings;

        public ExampleFunction(IHttpClientFactory httpClientFactory, IConfiguration config, IOptions<ExampleSettingsConfig> options)
        {
            _httpClient = httpClientFactory.CreateClient();
            _testUrl = config.GetValue<string>("TestUrl");
            _exampleTestSettings = options.Value;
        }

        [FunctionName(nameof(SimpleRequest))]
        public async Task<IActionResult> SimpleRequest([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("We have a logger!");
            log.LogInformation("The configured testUrl is: {testUrl}", _exampleTestSettings.TestUrl);

            var responseMessage = await _httpClient.GetAsync(_exampleTestSettings.TestUrl);
            responseMessage.EnsureSuccessStatusCode();

            var content = await responseMessage.Content.ReadAsStringAsync();
            log.LogInformation(content);

            return new OkResult();
        }
    }
}
