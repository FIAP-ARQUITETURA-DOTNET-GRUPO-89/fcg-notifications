using FcgNotifications.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;

namespace FcgNotifications.IntegrationTests.TestHelpers;

/// <summary>
/// Responsável por gerenciar o ciclo de vida do banco de dados utilizado nos testes de integração.
/// </summary>
public class TestDatabaseManager(string connectionString)
{
    private readonly string _connectionString = connectionString;
    private Respawner _respawner = default!;

    public async Task InitializeAsync()
    {
        await using var context = CreateDbContext();
        await context.Database.MigrateAsync();
        await InitializeRespawner();
    }

    public async Task ResetAsync()
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        await _respawner.ResetAsync(conn);
        await SeedAsync();
    }

    private async Task InitializeRespawner()
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        _respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
            TablesToIgnore = [new("__EFMigrationsHistory")]
        });
    }

    private async Task SeedAsync()
    {
        await using var context = CreateDbContext();
        await TestDataSeeder.SeedAsync(context);
    }

    private FcgNotificationsDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<FcgNotificationsDbContext>()
            .UseNpgsql(_connectionString)
            .Options;
        return new FcgNotificationsDbContext(options);
    }
}
