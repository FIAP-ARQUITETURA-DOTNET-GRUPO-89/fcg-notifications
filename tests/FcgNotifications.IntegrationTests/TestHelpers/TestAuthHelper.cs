using System.Net.Http.Headers;
using FcgNotifications.IntegrationTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace FcgNotifications.IntegrationTests.TestHelpers;

public static class TestAuthHelper
{
    public const string CustomerEmail = "user@fcgnotifications.com";
    public const string AdminEmail = "admin@fcgnotifications.com";

    public static async Task<HttpClient> CreateAdminClientAsync(IntegrationTestFixture fixture)
    {
        var client = fixture.CreateClient();
        var generator = fixture.App.Services.GetRequiredService<JwtTestTokenGenerator>();
        var token = generator.Generate(AdminEmail, "Admin");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    public static async Task<HttpClient> CreateUserCustomerAsync(IntegrationTestFixture fixture)
    {
        var client = fixture.CreateClient();
        var generator = fixture.App.Services.GetRequiredService<JwtTestTokenGenerator>();
        var token = generator.Generate(CustomerEmail, "Customer");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    public static HttpClient CreateAnonymousClient(IntegrationTestFixture fixture)
        => fixture.CreateClient();
}
