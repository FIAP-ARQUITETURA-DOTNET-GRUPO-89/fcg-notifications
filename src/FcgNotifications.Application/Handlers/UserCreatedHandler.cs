using FcgNotifications.Domain.Entities;
using FcgNotifications.Domain.Enums;
using FcgNotifications.Domain.Events;
using FcgNotifications.Domain.Repositories;
using FcgUsers.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FcgNotifications.Application.Handlers;

public sealed class UserCreatedHandler(
    INotificationRepository repository,
    ILogger<UserCreatedHandler> logger)
    : INotificationHandler<UserCreatedEvent>
{
    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var email = Email.Create(notification.Email);

        var entity = new Notification(
            notification.UserId,
            email,
            $"Bem-vindo, {notification.Name}!",
            NotificationType.Welcome);

        repository.Add(entity);
        await repository.SaveChangesAsync();

        try
        {
            logger.LogInformation("Simulando envio de e-mail de boas-vindas para: {Email}", email.Address);

            await Task.Delay(100, cancellationToken);

            entity.MarkAsSent();
            await repository.SaveChangesAsync();

            logger.LogInformation("Notificação de boas-vindas registrada com sucesso para o usuário {UserId}", notification.UserId);
        }
        catch (Exception ex)
        {
            
            logger.LogError(ex, "Erro ao enviar notificação para o usuário {UserId}", notification.UserId);

            entity.MarkAsFailed();
            await repository.SaveChangesAsync();

            throw;
        }
    }
}
