using System;
using System.IO;
using DataHubIntern.Db;
using DataHubIntern.EnrichFunctions;
using DataHubIntern.EnrichFunctions.ApiClient;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(StartUp))]

namespace DataHubIntern.EnrichFunctions;

public class StartUp : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = builder.GetContext().Configuration;
        builder.Services.AddDbContext<InternDbContext>(options =>
        {
            var path = Path.Combine(Environment.CurrentDirectory, "../../../../db_volume", "identifiedData.db");
            options.UseSqlite(@$"Data Source={path}");
        });
        builder.Services.AddHttpClients(configuration);
        builder.Services.AddSingleton<IRichInfoApiClient, RichInfoApiClient>();
    }
}