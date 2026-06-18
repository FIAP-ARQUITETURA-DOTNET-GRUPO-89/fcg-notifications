using FcgNotifications.Infrastructure.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FcgNotifications.UnitTests.TestHelpers.Factories;

public static class InMemoryDbContextFactory
{
    public static FcgNotificationsDbContext CreateContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<FcgNotificationsDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new FcgNotificationsDbContext(options);

        context.Database.EnsureCreated();

        return context;
    }
}
