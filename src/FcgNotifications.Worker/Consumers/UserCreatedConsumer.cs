using FcgNotifications.Application.Commands;
using FcgNotifications.Domain.Enums;
using FgcGames.EventContracts.Events;
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
        logger.LogInformation("UserCreatedConsumer recebeu UserId: {UserId}", context.Message.UserId);

        var createUserCommand = new CreateUserCommand(context.Message.UserId, context.Message.Name, context.Message.Email);
        await mediator.Send(createUserCommand, context.CancellationToken);

        var createNotificationCommand = new CreateNotificationCommand(context.Message.UserId, context.Message.Email, "COLOQUE AQUI A MENSAGEM", NotificationType.Welcome);
        await mediator.Send(createNotificationCommand, context.CancellationToken);
    }
}
