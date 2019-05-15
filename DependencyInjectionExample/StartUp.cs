using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(DependencyInjectionExample.Startup))]

namespace DependencyInjectionExample
{
    using DependencyInjectionExample.Config;
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            
            builder.Services.AddOptions<ExampleSettingsConfig>()
                .Configure<IConfiguration>((configSection, configuration) =>
                {
                    //Need to bind it manually to the section
                    //Heads up, you can not use nested sections in the local.settings.json. Use the ':' in your configuration keys.
                    configuration.GetSection("ExampleTestSettings").Bind(configSection);
                });
            
        
            //builder.Services.AddHttpClient("namedClient")
            //builder.Services.AddScoped<>();
            //builder.Services.AddTransient<>();
            //builder.Services.AddSingleton<>();
        }
    }
}
