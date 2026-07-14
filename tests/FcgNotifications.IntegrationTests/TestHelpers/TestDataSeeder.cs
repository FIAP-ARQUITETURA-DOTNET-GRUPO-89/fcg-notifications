using FcgNotifications.Domain.Entities;
using FcgNotifications.Domain.Enums;
using FcgUsers.Domain.ValueObjects;
using FcgNotifications.Infrastructure.Database;

namespace FcgNotifications.IntegrationTests.TestHelpers;

public static class TestDataSeeder
{
    public static async Task SeedAsync(FcgNotificationsDbContext context)
    {
        if (context.Notifications.Any())
        {
            return;
        }

        var notification = new Notification(
            Guid.NewGuid(),
            Email.Create("seed@sistema.com"),
            "Notificação de inicialização",
            NotificationType.Welcome
        );

        context.Notifications.Add(notification);
        await context.SaveChangesAsync();
    }
}
