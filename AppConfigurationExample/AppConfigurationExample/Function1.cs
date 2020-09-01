using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;

namespace AppConfigurationExample
{
    public class Function1
    {
        public Function1(IConfiguration configuration)
        {
            // you can use the injected IConfiguration that is now connected to Azure App Configuration
            // it's nicer to use IOptions<>
        }

        //public Function1(IOptions<ExampleSettingsConfig> exampleOptions)
        //{
        // It is better to use the options pattern
        //}

        [FunctionName("Function1")]
        public async Task<string> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            return "Serverless!";
        }
    }
}
