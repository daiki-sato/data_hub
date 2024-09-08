using System.Data;
using Azure.Storage.Queues;
using Dapper;
using DataHubIntern.Db;
using DataHubIntern.MinimumApi.ApiClient;
using DataHubIntern.MinimumApi.Repository;
using DataHubIntern.MinimumApi.Services;
using DataHubIntern.Shared.Enrich;
using DataHubIntern.Shared.File;
using DataHubIntern.Shared.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using ProtoBuf.Grpc.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InternDbContext>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IEnrichService, EnrichService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddLogging();

builder.Services.AddHttpClients(builder.Configuration);
builder.Services.AddSingleton<IIdentifyApiClient, IdentifyApiClient>();
builder.Services.AddSingleton<IRichInfoApiClient, RichInfoApiClient>();

var configuration = builder.Configuration;

builder.Services.AddAzureClients(x =>
    x.AddQueueServiceClient(configuration["QueueStorageConnectionStrings"])
        .ConfigureOptions(y => y.MessageEncoding = QueueMessageEncoding.Base64));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.WithOrigins("https://localhost:7273")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("x-user-agent", "grpc-status", "grpc-message");
    });
});

builder.Services.AddCodeFirstGrpc();

SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());

var app = builder.Build();

// Database migration
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<InternDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors();

app.UseGrpcWeb();
app.MapGrpcService<FileService>().EnableGrpcWeb();
app.MapGrpcService<EnrichService>().EnableGrpcWeb();
app.MapGrpcService<IdentityService>().EnableGrpcWeb();

app.Run();

file class DateTimeOffsetHandler : SqlMapper.TypeHandler<DateTimeOffset>
{
    public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
        => parameter.Value = value;

    public override DateTimeOffset Parse(object value)
        => DateTimeOffset.Parse((string)value);
}