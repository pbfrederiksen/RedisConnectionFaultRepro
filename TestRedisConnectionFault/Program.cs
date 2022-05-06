// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestRedisConnectionFault;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services.AddLogging()
        .AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379,password=Passw0rd,abortConnect=false";
            })
            .AddHttpClientsForExternalServices()
            .AddScoped<Cache.ICacheService, Cache.CacheService>())
    .Build();

using var serviceScope = host.Services.CreateScope();
var provider = serviceScope.ServiceProvider;

var cacheService = provider.GetService<Cache.ICacheService>();
var value = await cacheService.Get<string>("Dummy");

var httpClientFactory = provider.GetService<IHttpClientFactory>();

var httpClient = httpClientFactory.CreateClient("TestClient");
//httpClient = new HttpClient();
//httpClient.BaseAddress = new Uri("https://localhost:44333/");

try
{
    var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "api/v1/InternationalAppService/GetCarWashLocations"));
}
catch (Exception e)
{
    Console.WriteLine(e);
    //throw;
}
