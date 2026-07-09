using MediatR;

namespace FcgNotifications.Domain.Events;

public record PaymentProcessedEvent(
    Guid OrderId,
    Guid UserId,
    string CustomerEmail,
    string CustomerName,
    decimal TotalAmount,
    string Status
) : INotification;
