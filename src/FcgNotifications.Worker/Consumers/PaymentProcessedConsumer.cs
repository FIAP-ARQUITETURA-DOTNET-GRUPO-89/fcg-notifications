using FcgNotifications.Domain.Entities;
using FcgNotifications.Domain.Events;
using FcgNotifications.Domain.Enums;
using FcgUsers.Domain.ValueObjects;
using FcgNotifications.Infrastructure.Database;
using MassTransit;
using MediatR;

namespace FcgNotifications.Worker.Consumers;

public sealed partial class PaymentProcessedConsumer(
    IMediator mediator,
    FcgNotificationsDbContext dbContext,
    ILogger<PaymentProcessedConsumer> logger)
: IConsumer<PaymentProcessedEvent>
{
    public async Task Consume(ConsumeContext<PaymentProcessedEvent> context)
    {
        LogPaymentProcessedReceived(logger, context.Message.OrderId);

        var emailPlaceholder = Email.Create("sem-email@sistema.com");

        var notification = new Notification(
            context.Message.UserId,
            emailPlaceholder,
            $"Seu pedido {context.Message.OrderId} foi processado com sucesso!",
            NotificationType.PurchaseConfirmation
        );

        await dbContext.Notifications.AddAsync(notification);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Notificação de pagamento salva para o Pedido: {OrderId} (E-mail não disponível)", context.Message.OrderId);

        await mediator.Publish(context.Message);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Evento PaymentProcessed recebido para o Pedido: {OrderId}")]
    static partial void LogPaymentProcessedReceived(ILogger logger, Guid orderId);
}
