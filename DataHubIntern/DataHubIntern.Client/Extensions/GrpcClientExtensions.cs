using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using ProtoBuf.Grpc.Client;

namespace DataHubIntern.Client.Extensions;

public static class GrpcClientExtensions
{
    public static IServiceCollection AddGrpcService<TClient>(this IServiceCollection services, string address)
        where TClient : class
    {
        var handler = new GrpcWebHandler(new HttpClientHandler());
        var channel = GrpcChannel.ForAddress(address,
            new GrpcChannelOptions { HttpHandler = handler });

        var client = channel.CreateGrpcService<TClient>();
        services.AddSingleton(client);

        return services;
    }
}
