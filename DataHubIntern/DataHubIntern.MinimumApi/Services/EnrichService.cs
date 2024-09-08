using Azure.Storage.Queues;
using Dapper;
using DataHubIntern.Db;
using DataHubIntern.MinimumApi.ApiClient;
using Microsoft.EntityFrameworkCore;
using DataHubIntern.Shared.Enrich;

namespace DataHubIntern.MinimumApi.Services;

public class EnrichService(InternDbContext internDbContext, IRichInfoApiClient richInfoApiClient,
        QueueServiceClient queueServiceClient)
    : IEnrichService
{
    private readonly IRichInfoApiClient _richInfoApiClient = richInfoApiClient;

    public async Task EnrichOrganizationsAsync(CancellationToken cancellationToken)
    {

    }

    public async Task<GetEnrichedOrganizationsResponse> GetEnrichedOrganizationsAsync(CancellationToken cancellationToken)
    {
        return new GetEnrichedOrganizationsResponse { OrganizationRichInfos = new List<OrganizationRichInfo>() };
    }

    /// <summary>
    /// Dapper を使う際の挿入サンプルクエリ
    /// クエリの中でパラメーターを利用したい場合は @ をつけ、パラメーターオブジェクトを引数で与える。 (例：@Id)
    /// オブジェクトのパラメーター名とクエリ中のパラメーター名は一致する必要がある。 (@Id であれば Id である必要がある)
    /// 変更を与えるクエリは ExecuteAsync で行う。返り値は変更された行数。
    /// </summary>
    private async Task SampleInsertAsync(string id, string name)
    {
        await using var connection = internDbContext.Database.GetDbConnection();

        const string query = @"
INSERT INTO SampleTable
( Id, Name)
VALUES
( @Id, @Name )";

        var param = new
        {
            Id = id,
            Name = name
        };

        await connection.ExecuteAsync(query, param);
    }

    /// <summary>
    /// Dapper を使う際のサンプル選択クエリ
    /// クエリの中でパラメーターを利用したい場合は @ をつけ、パラメーターオブジェクトを引数で与える。 (例：@Id)
    /// オブジェクトのパラメーター名とクエリ中のパラメーター名は一致する必要がある。 (@Id であれば Id である必要がある)
    /// 選択クエリは QueryAsync<T> で行う。返り値は IEnumerable<T>。 T は取得するカラムの型。
    /// </summary>
    private async Task<IEnumerable<SampleEntity>> SampleQueryAsync(string id)
    {
        await using var connection = internDbContext.Database.GetDbConnection();

        const string query = @"
SELECT Id, Name FROM SampleTable
WHERE
    Id = @Id";

        var param = new
        {
            Id = id
        };

        var result = await connection.QueryAsync<SampleEntity>(query, param);
        return result;
    }

    /// <summary>
    /// StorageQueue にメッセージを送信する
    /// enrich キューのメッセージは DataHubIntern.Shared.EnrichMessage を Json シリアライズした文字列である必要がある。
    /// 適宜引数を追加して下さい。
    /// </summary>
    private async Task SendMessageToEnrichQueueAsync()
    {
        var queueClient = queueServiceClient.GetQueueClient("enrich");
        //Queue が存在しない場合は作成する
        await queueClient.CreateIfNotExistsAsync();
        var message = "Sample Message";
        await queueClient.SendMessageAsync(message);
    }

    //Sample 用の Entity です。
    //利用することはありません。
    private class SampleEntity
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}