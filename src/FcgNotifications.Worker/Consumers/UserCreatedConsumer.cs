using FcgNotifications.Domain.Entities;
using FcgNotifications.Domain.Events;
using FcgNotifications.Domain.Enums;
using FcgUsers.Domain.ValueObjects;
using FcgNotifications.Infrastructure.Database;
using MassTransit;
using MediatR;

namespace FcgNotifications.Worker.Consumers;

public sealed partial class UserCreatedConsumer(
    IMediator mediator,
    FcgNotificationsDbContext dbContext,
    ILogger<UserCreatedConsumer> logger)
: IConsumer<UserCreatedEvent>
{
    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        LogUserCreatedReceived(logger, context.Message.UserId, context.Message.Email);

        var email = Email.Create(context.Message.Email);

        var notification = new Notification(
            context.Message.UserId,
            email,
            "Bem-vindo à nossa plataforma!",
            NotificationType.Welcome
        );

        await dbContext.Notifications.AddAsync(notification);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Notificação salva para o UserId: {UserId}", context.Message.UserId);

        await mediator.Publish(context.Message);
    }

    [LoggerMessage(
        EventId = 2002,
        Level = LogLevel.Information,
        Message = "Evento UserCreated recebido. UserId: {UserId}, Email: {Email}")]
    static partial void LogUserCreatedReceived(ILogger logger, Guid userId, string email);
}
