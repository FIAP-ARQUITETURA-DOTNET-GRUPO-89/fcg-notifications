using FcgNotifications.Application.Commands;
using FcgNotifications.Domain.Entities;
using FcgNotifications.Domain.Enums;
using FcgNotifications.Domain.Repositories;
using FcgUsers.Domain.ValueObjects;
using FgcGames.EventContracts.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using OperationResult;

namespace FcgNotifications.Application.Handlers;

public sealed class PaymentProcessedHandler(
    INotificationRepository repository,
    IUserRepository userRepository,
    ILogger<PaymentProcessedHandler> logger)
: IRequestHandler<ProcessPaymentResultCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ProcessPaymentResultCommand command, CancellationToken cancellationToken)
    {
        if (command.Status != PaymentStatus.Approved)
        {
            return new Exception();
        }

        var email = Email.Create("email@email.com");

        var entity = new Notification(
            command.UserId,
            email,
            $"Olá UserName, seu pagamento do pedido {command.OrderId} foi aprovado!",
            NotificationType.PurchaseConfirmation);

        repository.Add(entity);
        await repository.SaveChangesAsync();

        logger.LogInformation("Notificação de confirmação enviada para pedido {OrderId}", command.OrderId);

        entity.MarkAsSent();
        await repository.SaveChangesAsync();

        return Result.Success(true);
    }
}
