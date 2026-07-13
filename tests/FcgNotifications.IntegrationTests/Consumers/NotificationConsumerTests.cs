using Aspire.Hosting.Testing;
using FcgNotifications.Domain.Enums;
using FcgNotifications.Domain.Events;
using FcgNotifications.IntegrationTests.Fixtures;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace FcgNotifications.IntegrationTests.Tests;

[Collection("IntegrationTests")]
public class NotificationConsumerTests(IntegrationTestFixture fixture)
{
    private readonly IntegrationTestFixture _fixture = fixture;

    [Fact]
    public async Task Deve_Criar_Notificacao_Ao_Consumir_UserCreatedEvent()
    {
        var rabbitConnectionString = await _fixture.App.GetConnectionStringAsync("rabbitmq");

        var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(rabbitConnectionString);
        });

        await busControl.StartAsync();

        var userId = Guid.NewGuid();
        var message = new UserCreatedEvent(userId, "Teste", "teste@fiap.com", DateTime.UtcNow);

        await busControl.Publish(message);
        await Task.Delay(4000);

        var notification = await _fixture.ExecuteDbContextAsync(async db =>
            await db.Notifications.FirstOrDefaultAsync(n => n.UserId == userId));

        notification.ShouldNotBeNull();

        await busControl.StopAsync();
    }

    [Fact]
    public async Task Deve_Criar_Notificacao_Ao_Consumir_PaymentProcessedEvent_Aprovado()
    {
        var rabbitConnectionString = await _fixture.App.GetConnectionStringAsync("rabbitmq");

        var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(rabbitConnectionString);
        });

        await busControl.StartAsync();

        var orderId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var message = new PaymentProcessedEvent(orderId, userId, "cliente@teste.com", "Cliente Teste", 100.00m, "Approved");

        await busControl.Publish(message);

        await Task.Delay(4000);

        var notification = await _fixture.ExecuteDbContextAsync(async db =>
            await db.Notifications.FirstOrDefaultAsync(n => n.UserId == userId && n.Type == NotificationType.PurchaseConfirmation));

        notification.ShouldNotBeNull();
        notification.Message.ShouldContain(orderId.ToString());

        await busControl.StopAsync();
    }
}
