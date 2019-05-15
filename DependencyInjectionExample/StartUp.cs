using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using DependencyInjectionExample.Config;

[assembly: FunctionsStartup(typeof(DependencyInjectionExample.Startup))]

namespace DependencyInjectionExample
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddOptions<ExampleSettingsConfig>()
                .Configure<IConfiguration>((configSection, configuration) =>
                {
                    configuration.GetSection("ExampleTestSettings").Bind(configSection);
                });
            
        
            //builder.Services.AddHttpClient("namedClient")
            //builder.Services.AddScoped<>();
            //builder.Services.AddTransient<>();
            //builder.Services.AddSingleton<>();
        }
    }

  
}
