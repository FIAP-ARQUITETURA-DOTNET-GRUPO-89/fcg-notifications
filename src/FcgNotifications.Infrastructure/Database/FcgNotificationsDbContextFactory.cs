using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FcgNotifications.Infrastructure.Database;

public class FcgNotificationsDbContextFactory : IDesignTimeDbContextFactory<FcgNotificationsDbContext>
{
    public FcgNotificationsDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../FcgNotifications.Worker"))
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<FcgNotificationsDbContext>();
        var connectionString = configuration.GetConnectionString("Default");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("A Connection String 'Default' não foi encontrada. Verifique o appsettings.json do Worker.");
        }

        optionsBuilder.UseNpgsql(connectionString);

        return new FcgNotificationsDbContext(optionsBuilder.Options);
    }
}
