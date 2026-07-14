using FcgNotifications.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace FcgNotifications.Worker.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

        var runMigration = environment.IsDevelopment() || (environment.IsProduction() && Environment.GetEnvironmentVariable("RUN_MIGRATION") == "true");

        if (!runMigration)
        {
            return;
        }

        var db = scope.ServiceProvider.GetRequiredService<FcgNotificationsDbContext>();

        if (db.Database.IsRelational())
        {
            await db.Database.MigrateAsync();
        }
    }
}
