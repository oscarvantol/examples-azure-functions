using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DurableFunctionDemo
{
    public static class TaskRaceFunction
    {
        static Random _rand = new Random();

        [FunctionName("TaskRaceFunction")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            var grid = new List<Task<(string, TimeSpan)>>();

            // Replace "hello" with the name of your Durable Activity Function.
            grid.Add(context.CallActivityAsync<(string, TimeSpan)>("TaskRaceFunction_Race", "Ham"));
            grid.Add(context.CallActivityAsync<(string, TimeSpan)>("TaskRaceFunction_Race", "Ver"));
            grid.Add(context.CallActivityAsync<(string, TimeSpan)>("TaskRaceFunction_Race", "Bot"));

            var winner = await Task.WhenAny(grid);
            log.LogInformation($"Race winner is: {winner.Result}!");

            await Task.WhenAll(grid); //finished or crashed
            log.LogInformation("Race finished!");

            //Show results
            grid.OrderBy(g => g.Result.Item2)
                .Select((x, index) => (driver: x.Result.Item1, duration: x.Result.Item2, position: index + 1))
                .ToList()
                .ForEach((x) => { log.LogInformation($"{x.position}: {x.driver} ({x.duration})"); });
        }

        [FunctionName("TaskRaceFunction_Race")]
        public static async Task<(string driver, TimeSpan duration)> SayHello([ActivityTrigger] string driver, ILogger log)
        {
            log.LogInformation($"{driver} has started the race!");
            var randomTime = TimeSpan.FromMilliseconds(_rand.Next(7000, 9500));
            await Task.Delay(randomTime);
            return (driver, randomTime);
        }

        [FunctionName("TaskRaceFunction_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("TaskRaceFunction", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}