using FcgNotifications.Application.Commands;
using FcgNotifications.Domain.Entities;
using FcgNotifications.Domain.Enums;
using FcgNotifications.Domain.Repositories;
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
            return Result.Error<bool>(new Exception("Pagamento não aprovado."));

        var user = await userRepository.GetByIdAsync(command.UserId);
        if (user == null)
            return Result.Error<bool>(new Exception("Usuário não encontrado."));

        var entity = new Notification(
            command.UserId,
            user.Email,
            $"Olá {user.Name}, seu pagamento do pedido {command.OrderId} foi aprovado!",
            NotificationType.PurchaseConfirmation);

        repository.Add(entity);
        await repository.SaveChangesAsync();

        entity.MarkAsSent();
        await repository.SaveChangesAsync();

        return Result.Success(true);
    }
}
