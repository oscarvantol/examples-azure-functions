using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DurableFunctionDemo
{
    public class ApprovalFunction
    {
        [FunctionName("SupportRequest_HttpStart")]
        public async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string instanceId = await starter.StartNewAsync("SupportRequest", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        [FunctionName("SupportRequest")]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            //await context.CallActivityAsync("SupportRequest_LogHelpLink", context.InstanceId);

            //Task<bool> requestApprovalTask = context.WaitForExternalEvent<bool>("RequestApproval");

            //using var timeoutCts = new CancellationTokenSource();
            //var timeoutTask = context.CreateTimer(context.CurrentUtcDateTime.AddSeconds(200), timeoutCts.Token);

            //var completedTask = await Task.WhenAny(requestApprovalTask, timeoutTask);

            //if (completedTask == requestApprovalTask)
            //{
            //    if (requestApprovalTask.Result)
            //    {
            //        await context.CallActivityAsync("SupportRequest_ExecuteRequest", "");
            //    }
            //    else
            //    {
            //        log.LogError("Request denied!");
            //    }
            //}
            //else
            //{
            //    log.LogError("Request timed out.");
            //}

            //if (!timeoutTask.IsCompleted)
            //{
            //    timeoutCts.Cancel();
            //}
        }

        [FunctionName("SupportRequest_LogHelpLink")]
        public static async Task LogHelpLink([ActivityTrigger] string instanceId, ILogger log)
        {
            log.LogInformation("\n\n\n===== Example Callback =====");
            log.LogInformation($"curl -d true -H 'Content-Type: application/json' http://localhost:7071/runtime/webhooks/durabletask/instances/{instanceId}/raiseEvent/RequestApproval?taskHub=TestHubName&connection=Storage");
            log.LogInformation("\n\n\n===== Example Callback =====");
        }

        [FunctionName("SupportRequest_ExecuteRequest")]
        public static async Task ExecuteSupportRequest([ActivityTrigger] string request, ILogger log)
        {
            log.LogInformation($"\n\n\n\n\t\tExecuting specific request.\n\n\n\n\n\n\n");
        }


    }
}