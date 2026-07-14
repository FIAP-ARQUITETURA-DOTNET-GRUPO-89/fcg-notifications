using FcgNotifications.Application.Commands;
using FcgNotifications.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using OperationResult;

namespace FcgNotifications.Application.Handlers;

public sealed class CreateNotificationHandler(
    INotificationRepository repository,
    ILogger<CreateNotificationCommand> logger)
: IRequestHandler<CreateNotificationCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateNotificationCommand command, CancellationToken cancellationToken)
    {
        logger.LogDebug("CRIANDO EVENTO NO BANCO");
        // COLOCAR AQUI LÓGICA DE SALVAR NOTICAÇÃO EM BANCO

        return Result.Success(true);
    }
}
