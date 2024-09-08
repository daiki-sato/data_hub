using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataHubIntern.EnrichFunctions.ApiClient;

public static class HttpClientFactoryExtensions
{
    public static void AddHttpClients(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient(RichInfoApiClient.ClientName, (_, c) =>
        {
            c.BaseAddress = new Uri(config["ApiEndpoint"]);
            c.DefaultRequestHeaders.Add("x-functions-key", config["EnrichApiKey"]);
        });
    }
}

