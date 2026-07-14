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
        var rabbitConnectionString = await _fixture.App.GetConnectionStringAsync("rabbitmq", cancellationToken: TestContext.Current.CancellationToken);

        var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(rabbitConnectionString);
        });

        await busControl.StartAsync(TestContext.Current.CancellationToken);

        await Task.Delay(5000, TestContext.Current.CancellationToken);

        var userId = Guid.NewGuid();
        var email = "teste@fiap.com";

        var message = new UserCreatedEvent(
            userId,
            "Teste",
            email,
            DateTime.UtcNow);

        await busControl.Publish(message, TestContext.Current.CancellationToken);

        await Task.Delay(4000, TestContext.Current.CancellationToken);

        var notification = await _fixture.ExecuteDbContextAsync(async db =>
            await db.Notifications.FirstOrDefaultAsync(n => n.UserId == userId));

        notification.ShouldNotBeNull();
        notification.UserEmail.Address.ShouldBe(email);
        notification.Type.ShouldBe(NotificationType.Welcome);

        await busControl.StopAsync(TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task Deve_Criar_Notificacao_Ao_Consumir_PaymentProcessedEvent_Aprovado()
    {
        var rabbitConnectionString = await _fixture.App.GetConnectionStringAsync("rabbitmq", cancellationToken: TestContext.Current.CancellationToken);

        var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(rabbitConnectionString);
        });

        await busControl.StartAsync(TestContext.Current.CancellationToken);

        var orderId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var customerEmail = "cliente@teste.com";

        var message = new PaymentProcessedEvent(
            orderId,
            userId,
            customerEmail,
            "Cliente Teste",
            100.00m,
            "Approved");

        await busControl.Publish(message, TestContext.Current.CancellationToken);

        await Task.Delay(4000, TestContext.Current.CancellationToken);

        var notification = await _fixture.ExecuteDbContextAsync(async db =>
            await db.Notifications.FirstOrDefaultAsync(n =>
                n.UserId == userId &&
                n.Type == NotificationType.PurchaseConfirmation));

        notification.ShouldNotBeNull();
        notification.UserEmail.Address.ShouldBe(customerEmail);
        notification.Message.ShouldContain(orderId.ToString());

        await busControl.StopAsync(TestContext.Current.CancellationToken);
    }
}
