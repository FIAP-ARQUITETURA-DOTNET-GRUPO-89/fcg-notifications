using FcgNotifications.Domain.Entities;
using FcgNotifications.Domain.Enums;
using FcgNotifications.Domain.Events;
using FcgNotifications.Domain.Repositories;
using FcgUsers.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FcgNotifications.Application.Handlers;

public sealed class PaymentProcessedHandler(
    INotificationRepository repository,
    ILogger<PaymentProcessedHandler> logger)
    : INotificationHandler<PaymentProcessedEvent>
{
    public async Task Handle(PaymentProcessedEvent notification, CancellationToken cancellationToken)
    {
        if (notification.Status != "Approved") return;

        var email = Email.Create(notification.CustomerEmail);

        var entity = new Notification(
            notification.UserId,
            email,
            $"Olá {notification.CustomerName}, seu pagamento do pedido {notification.OrderId} foi aprovado!",
            NotificationType.PurchaseConfirmation);

        repository.Add(entity);
        await repository.SaveChangesAsync();

        logger.LogInformation("Notificação de confirmação enviada para pedido {OrderId}", notification.OrderId);

        entity.MarkAsSent();
        await repository.SaveChangesAsync();
    }
}
