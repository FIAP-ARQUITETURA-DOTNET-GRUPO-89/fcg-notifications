using FcgNotifications.Domain.Events;
using MassTransit;
using MediatR;

namespace FcgNotifications.Worker.Consumers;

public sealed partial class UserCreatedConsumer(
    IMediator mediator,
    ILogger<UserCreatedConsumer> logger)
: IConsumer<UserCreatedEvent>
{
    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        LogUserCreatedReceived(logger, context.Message.UserId, context.Message.Email);

        await mediator.Publish(context.Message);
    }

    [LoggerMessage(
        EventId = 2002,
        Level = LogLevel.Information,
        Message = "Evento UserCreated recebido. UserId: {UserId}, Email: {Email}")]
    static partial void LogUserCreatedReceived(ILogger logger, Guid userId, string email);
}
