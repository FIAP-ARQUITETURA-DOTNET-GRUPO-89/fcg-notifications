using FcgNotifications.Application.Commands;
using FcgNotifications.Domain.Enums;
using FgcGames.EventContracts.Events;
using MassTransit;
using MediatR;

namespace FcgNotifications.Worker.Consumers;

public sealed class UserCreatedConsumer(
    IMediator mediator,
    ILogger<UserCreatedConsumer> logger)
    : IConsumer<UserCreatedEvent>
{
    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var eventData = context.Message;

        logger.LogInformation("Processando UserCreatedEvent para UserId: {UserId}", eventData.UserId);

        var createUserCommand = new CreateUserCommand(
            eventData.UserId,
            eventData.Name,
            eventData.Email);

        await mediator.Send(createUserCommand, context.CancellationToken);

        var welcomeMessage = $"Olá {eventData.Name}, bem-vindo à plataforma FCG Games!";

        var createNotificationCommand = new CreateNotificationCommand(
            eventData.UserId,
            eventData.Email,
            welcomeMessage,
            NotificationType.Welcome);

        await mediator.Send(createNotificationCommand, context.CancellationToken);

        logger.LogInformation("Usuário {UserId} registrado e notificação de boas-vindas criada com sucesso.", eventData.UserId);
    }
}
