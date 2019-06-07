using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;

[assembly: FunctionsStartup(typeof(HttpClientFactoryWithPollyExample.Startup))]
namespace HttpClientFactoryWithPollyExample
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient("HttpStat", client =>
            {
                client.BaseAddress = new Uri("http://httpstat.us/");
            })
            .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync((new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            }), onRetry: (httpResponseMessage, timespan) =>
            {
                //Add some magic or logging here
            }));

            //checkout https://github.com/App-vNext/Polly/wiki/Polly-and-HttpClientFactory for all kinds of patterns
        }
    }
}
