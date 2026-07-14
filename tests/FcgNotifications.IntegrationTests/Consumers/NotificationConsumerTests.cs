using Aspire.Hosting.Testing;
using FcgNotifications.Domain.Entities;
using FcgNotifications.Domain.Enums;
using FcgNotifications.IntegrationTests.Fixtures;
using FcgUsers.Domain.ValueObjects;
using FgcGames.EventContracts.Enums;
using FgcGames.EventContracts.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace FcgNotifications.IntegrationTests.Tests;

[Collection("IntegrationTests")]
public class NotificationConsumerTests(IntegrationTestFixture fixture) : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture = fixture;

    public async ValueTask InitializeAsync() => await _fixture.ResetDatabaseAsync();
    public async ValueTask DisposeAsync() => await ValueTask.CompletedTask;

    private async Task<Notification?> WaitForNotificationAsync(Guid userId, int timeoutMs = 8000)
    {
        var start = DateTime.UtcNow;
        while ((DateTime.UtcNow - start).TotalMilliseconds < timeoutMs)
        {
            var notification = await _fixture.ExecuteDbContextAsync(async db =>
                await db.Notifications.FirstOrDefaultAsync(n => n.UserId == userId));

            if (notification != null) return notification;
            await Task.Delay(500);
        }
        return null;
    }

    [Fact]
    public async Task Deve_Criar_Notificacao_Ao_Consumir_UserCreatedEvent()
    {
        var rabbitConnectionString = await _fixture.App.GetConnectionStringAsync("rabbitmq");
        var busControl = Bus.Factory.CreateUsingRabbitMq(cfg => cfg.Host(rabbitConnectionString));
        await busControl.StartAsync();

        var userId = Guid.NewGuid();
        var email = "teste@fiap.com";
        var message = new UserCreatedEvent(userId, "Teste", email, DateTime.UtcNow);

        await busControl.Publish(message);

        var notification = await WaitForNotificationAsync(userId);

        notification.ShouldNotBeNull();
        notification.UserEmail.Address.ShouldBe(email);
        notification.Type.ShouldBe(NotificationType.Welcome);

        await busControl.StopAsync();
    }

    [Fact]
    public async Task Deve_Criar_Notificacao_Ao_Consumir_PaymentProcessedEvent_Aprovado()
    {
        // Arrange
        var rabbitConnectionString = await _fixture.App.GetConnectionStringAsync("rabbitmq");
        var busControl = Bus.Factory.CreateUsingRabbitMq(cfg => cfg.Host(rabbitConnectionString));
        await busControl.StartAsync();

        var orderId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var customerEmail = "cliente@teste.com";

        await _fixture.ExecuteDbContextAsync(async db => {
            var user = new User(userId, "Cliente Teste", Email.Create(customerEmail));
            db.Users.Add(user);
            return await db.SaveChangesAsync();
        });

        var message = new PaymentProcessedEvent(
            orderId,
            userId,
            Guid.NewGuid(),
            100.00m,
            PaymentStatus.Approved,
            DateTime.UtcNow);

        // Act
        var endpoint = await busControl.GetSendEndpoint(new Uri("queue:notifications-payment-processed-events"));
        await endpoint.Send(message);

        var notification = await WaitForNotificationAsync(userId);

        // Assert
        notification.ShouldNotBeNull("A notificação não foi criada. Verifique no Dashboard do Aspire os logs do worker.");
        notification.UserEmail.Address.ShouldBe(customerEmail);
        notification.Message.ShouldContain(orderId.ToString());

        await busControl.StopAsync();
    }
}
