using System.Net;
using ProtoBuf;
using ProtoBuf.Grpc.Configuration;

namespace DataHubIntern.Shared.File;

[Service]
public interface IFileService
{
    [Operation]
    Task<UploadResponse> UploadAsync(IAsyncEnumerable<UploadRequest> request, CancellationToken cancellationToken = default);
}

[ProtoContract]
public class UploadRequest
{
    [ProtoMember(1)]
    public string Name { get; set; }

    [ProtoMember(2)]
    public byte[] ChunkData { get; set; }
}

[ProtoContract]
public class UploadResponse
{
    [ProtoMember(1)]
    public HttpStatusCode StatusCode { get; set; }

    [ProtoMember(2)] 
    public List<string> Successes { get; set; } = new();

    [ProtoMember(3)]
    public Dictionary<int, List<string>> Errors { get; set; } = new();
}