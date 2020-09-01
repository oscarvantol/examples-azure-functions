using System;
using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: FunctionsStartup(typeof(AppConfigurationExample.StartUp))]
namespace AppConfigurationExample
{
    public class StartUp : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //get the original configuration
            var tmpConfig = GetConfiguration(builder);

            // create a new configurationbuilder and add appconfiguration
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddAzureAppConfiguration((options) =>
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

            // you configure other configuration sources here
            configBuilder.AddEnvironmentVariables();

            // replace
            builder.Services.RemoveAll<IConfiguration>();
            builder.Services.AddSingleton<IConfiguration>(configBuilder.Build());

            // continue the normal setup
            ConfigureServices(builder.Services);
        }

        private void ConfigureServices(IServiceCollection services)
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

        private IConfiguration GetConfiguration(IFunctionsHostBuilder builder) =>
            builder.Services.BuildServiceProvider().GetService<IConfiguration>();

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
