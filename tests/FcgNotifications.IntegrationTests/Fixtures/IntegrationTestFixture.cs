using Aspire.Hosting;
using Aspire.Hosting.Testing;
using FcgNotifications.Infrastructure.Database;
using FcgNotifications.IntegrationTests.TestHelpers;
using Microsoft.EntityFrameworkCore;

namespace FcgNotifications.IntegrationTests.Fixtures;

public class IntegrationTestFixture : IAsyncLifetime
{
    public DistributedApplication App { get; private set; } = default!;
    private TestDatabaseManager _dbManager = default!;
    private string _connectionString = string.Empty;

    public async ValueTask InitializeAsync()
    {
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Testing");
        var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.FcgNotifications_AppHost>();

        App = await builder.BuildAsync();
        await App.StartAsync();

        _connectionString = await App.GetConnectionStringAsync("Default")
            ?? throw new InvalidOperationException("Connection string não encontrada.");

        _dbManager = new TestDatabaseManager(_connectionString);
        await _dbManager.InitializeAsync();
    }

    public HttpClient CreateClient() => App.CreateHttpClient("fcgnotifications-worker");

    public async Task<T> ExecuteDbContextAsync<T>(Func<FcgNotificationsDbContext, Task<T>> action)
    {
        var options = new DbContextOptionsBuilder<FcgNotificationsDbContext>()
            .UseNpgsql(_connectionString).Options;
        await using var context = new FcgNotificationsDbContext(options);
        return await action(context);
    }

    public async ValueTask DisposeAsync()
    {
        await App.StopAsync();
        await App.DisposeAsync();
    }
}
