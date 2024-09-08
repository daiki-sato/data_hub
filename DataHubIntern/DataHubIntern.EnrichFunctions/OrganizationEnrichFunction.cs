using System.Threading;
using System.Threading.Tasks;
using DataHubIntern.Db;
using DataHubIntern.EnrichFunctions.ApiClient;
using DataHubIntern.Shared.Messages;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DataHubIntern.EnrichFunctions;

public class OrganizationEnrichFunction(InternDbContext internDbContext, IRichInfoApiClient richInfoApiClient)
{
    private readonly InternDbContext _internDbContext = internDbContext;
    private readonly IRichInfoApiClient _richInfoApiClient = richInfoApiClient;

    [FunctionName("OrganizationEnrichFunction")]
    public async Task RunAsync([QueueTrigger("enrich", Connection = "AzureWebJobsStorage")]string rawMessage, ILogger log, CancellationToken cancellationToken)
    {
        //メッセージは Json シリアライズされているので、デシリアライズする
        var message = JsonConvert.DeserializeObject<EnrichMessage>(rawMessage);
    }
}