namespace DataHubIntern.MinimumApi.ApiClient;

public static class HttpClientFactoryExtensions
{
    public static void AddHttpClients(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient(IdentifyApiClient.ClientName, (provider, c) =>
        {
            c.BaseAddress = new Uri(config["ApiEndpoint"]);
            c.DefaultRequestHeaders.Add("x-functions-key", config["IdentifyApiKey"]);
        });

        services.AddHttpClient(RichInfoApiClient.ClientName, (provider, c) =>
        {
            c.BaseAddress = new Uri(config["ApiEndpoint"]);
            c.DefaultRequestHeaders.Add("x-functions-key", config["EnrichApiKey"]);
        });
    }
}
