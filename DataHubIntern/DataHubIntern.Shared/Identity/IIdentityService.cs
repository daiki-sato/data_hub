using DataHubIntern.Shared.File;
using ProtoBuf;
using ProtoBuf.Grpc.Configuration;

namespace DataHubIntern.Shared.Identity;

[Service]
public interface IIdentityService
{

    [Operation]
    Task<ListResponse> ListAsync(CancellationToken cancellationToken = default);

    [Operation]
    Task<ListBySocResponse> ListBySocAsync(ListBySocRequest request, CancellationToken cancellationToken = default);

    [Operation]
    Task<Identity> IdentifyAsync(RawData record, CancellationToken cancellationToken = default);
}

[ProtoContract]
public class ListResponse
{
    [ProtoMember(1)]
    public string Status { get; set; }

    [ProtoMember(2)]
    public List<Identity> Identities { get; set; } = new();
}

[ProtoContract]
public class ListBySocRequest
{
    [ProtoMember(1)]
    public string UniqueKey { get; set; }
}

[ProtoContract]
public class ListBySocResponse
{
    [ProtoMember(1)]
    public string Status { get; set; }

    [ProtoMember(2)]
    public List<Identity> Identities { get; set; } = new();
}
