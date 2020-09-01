# examples-azure-functions

**AppConfigurationExample**

An example to add Azure App Configuation to Azure Functions using the FunctionsStartup. This example also configures Azure Key Vault for Key Vault reference usages and assumes using Azure.Identity, therefore the configuration only needs en endpoint. No secrets at all!

*Note: The configuration that you need for your triggers and bindings should cannot be used from appconfiguration because it is needed by the Azure Functions platform to for e.g. determine scaling.*


**DependencyInjectionExample**

Using 'FunctionsStartup' for DI. The example shows how to inject the IHttpClientFactory and setting up configuration using both IConfiguration and IOptions.

Based on the [Official docs](https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection)

[2019-12-30 - Updated to Functions v3, .net core 3 and csharp 8.]

**HttpClientFactoryWithPollyExample**

Simple demonstration how to setup Polly in Azure Functions 2 to handle retries or other policies.

Follow the link to explore [Polly and the HttpClientFactory](https://github.com/App-vNext/Polly/wiki/Polly-and-HttpClientFactory)
