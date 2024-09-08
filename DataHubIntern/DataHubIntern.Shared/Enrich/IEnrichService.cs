using ProtoBuf;
using ProtoBuf.Grpc.Configuration;

namespace DataHubIntern.Shared.Enrich;

[Service]
public interface IEnrichService
{
    [Operation]
    public Task EnrichOrganizationsAsync(CancellationToken cancellationToken);

    [Operation]
    public Task<GetEnrichedOrganizationsResponse> GetEnrichedOrganizationsAsync(CancellationToken cancellationToken);
}

[ProtoContract]
public record GetEnrichedOrganizationsResponse
{
    [ProtoMember(1)]
    public List<OrganizationRichInfo> OrganizationRichInfos { get; set; } = new();
}