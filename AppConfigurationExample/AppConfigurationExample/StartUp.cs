using System;
using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: FunctionsStartup(typeof(AppConfigurationExample.StartUp))]
namespace AppConfigurationExample
{
    public class StartUp : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            //get the original configuration
            var tmpConfig = builder.ConfigurationBuilder.Build();

            // create a new configurationbuilder and add appconfiguration
            builder.ConfigurationBuilder.AddAzureAppConfiguration((options) =>
            {
                var defaultAzureCredential = GetDefaultAzureCredential();

                options.Connect(new Uri(tmpConfig["AppConfigUrl"]), defaultAzureCredential)
                // also setup key vault for key vault references
                    .ConfigureKeyVault(kvOptions =>
                    {
                        kvOptions.SetCredential(defaultAzureCredential);
                    });

                // configure appconfiguation features you want;
                // options.UseFeatureFlags();
                // options.Select(KeyFilter.Any, LabelFilter.Null);
            });

        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            //services.AddOptions<ExampleSettingsConfig>()
            // .Configure<IConfiguration>((configSection, configuration) =>
            // {
            //        configuration.GetSection("ExampleTestSettings").Bind(configSection);
            // });

            //builder.Services.AddHttpClient("namedClient")
            //builder.Services.AddScoped<>();
            //builder.Services.AddTransient<>();
            //builder.Services.AddSingleton<>();
        }

        private DefaultAzureCredential GetDefaultAzureCredential() => new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            //be explicit about this to prevent frustration
            ExcludeManagedIdentityCredential = false,
            ExcludeAzureCliCredential = false,

            ExcludeSharedTokenCacheCredential = true,
            ExcludeVisualStudioCodeCredential = true,
            ExcludeInteractiveBrowserCredential = true,
            ExcludeEnvironmentCredential = true,
            ExcludeVisualStudioCredential = true
        });
    }
}
