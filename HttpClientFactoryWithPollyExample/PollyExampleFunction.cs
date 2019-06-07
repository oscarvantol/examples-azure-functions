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

namespace HttpClientFactoryWithPollyExample
{
    public class PollyExampleFunction
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PollyExampleFunction(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [FunctionName(nameof(CallAFailingEndpointAndRetry))]
        public async Task<IActionResult> CallAFailingEndpointAndRetry(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //httpstat.us has endpoints for all http status codes, calling a 503 will handle the transient error configured with polly in the startup
            var client = _httpClientFactory.CreateClient("HttpStat");
            var _ = await client.GetStringAsync("/503");

            return new OkResult();
        }
    }
}
