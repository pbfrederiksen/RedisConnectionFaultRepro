using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;

namespace TestRedisConnectionFault;

public static class HttpClientsExtension
{
    public static IServiceCollection AddHttpClientsForExternalServices(this IServiceCollection services)
    {
        services.AddAccessTokenManagement((sp, options) =>
        {
            options.Client.Clients.Add("TestClient", new ClientCredentialsTokenRequest
            {
                Address = "localhost:8080",
                ClientId = "SECRET",
                ClientSecret = "SECRET",
                Scope = "SECRET"
            });
        });

        services.AddHttpClient("TestClient", (sp, client) =>
        {
            client.BaseAddress = new Uri("https://localhost:44333/");
        }).AddClientAccessTokenHandler("TestClient");

        return services;
    }
}