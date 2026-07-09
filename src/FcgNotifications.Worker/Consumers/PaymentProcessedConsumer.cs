using FcgNotifications.Domain.Events;
using MassTransit;
using MediatR;

namespace FcgNotifications.Worker.Consumers;

public sealed partial class PaymentProcessedConsumer(
    IMediator mediator,
    ILogger<PaymentProcessedConsumer> logger)
: IConsumer<PaymentProcessedEvent> // Deve consumir PaymentProcessedEvent
{
    public async Task Consume(ConsumeContext<PaymentProcessedEvent> context)
    {
        LogPaymentProcessedReceived(logger, context.Message.OrderId);
        await mediator.Publish(context.Message);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Evento PaymentProcessed recebido para o Pedido: {OrderId}")]
    static partial void LogPaymentProcessedReceived(ILogger logger, Guid orderId);
}
