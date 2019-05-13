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

namespace DependencyInjectionExample
{
    public class ExampleFunction
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExampleFunction(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [FunctionName(nameof(SimpleRequest))]
        public async Task<IActionResult> SimpleRequest([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("We have a logger!");


            _ = await _httpClientFactory.CreateClient("test").GetAsync(@"https://jsonplaceholder.typicode.com/todos");


            return new OkResult();
        }
    }
}
