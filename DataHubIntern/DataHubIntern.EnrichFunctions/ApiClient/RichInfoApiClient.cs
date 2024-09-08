using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using DataHubIntern.EnrichFunctions.ApiClient.Models;

namespace DataHubIntern.EnrichFunctions.ApiClient;

public interface IRichInfoApiClient
{
    public Task<RichInfoResponse> GetRichInfoAsync(string soc, CancellationToken cancellationToken);
}

public class RichInfoApiClient : IRichInfoApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    public const string ClientName = "RichInfoApi";

    public RichInfoApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<RichInfoResponse> GetRichInfoAsync(string soc, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient(ClientName);
        return new RichInfoResponse("", "", 0, 0);
    }
}