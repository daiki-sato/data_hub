using DataHubIntern.Client;
using DataHubIntern.Client.Extensions;
using DataHubIntern.Client.Services;
using DataHubIntern.Shared.Enrich;
using DataHubIntern.Shared.File;
using DataHubIntern.Shared.Identity;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBootstrapBlazor();

builder.Services.AddGrpcService<IIdentityService>("https://localhost:7090");
builder.Services.AddGrpcService<IFileService>("https://localhost:7090");
builder.Services.AddGrpcService<IEnrichService>("https://localhost:7090");

builder.Services.AddSingleton<ICsvValidator, CsvHeaderValidator>();

await builder.Build().RunAsync();