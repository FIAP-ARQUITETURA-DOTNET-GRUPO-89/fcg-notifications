using FcgNotifications.Application.Commands;
using FcgNotifications.Application.Handlers;
using FcgNotifications.Domain.Entities;
using FcgNotifications.Domain.Enums;
using FcgNotifications.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;

public class CreateNotificationHandlerTests
{
    private readonly INotificationRepository _repository = Substitute.For<INotificationRepository>();
    private readonly CreateNotificationHandler _sut;

    public CreateNotificationHandlerTests() =>
        _sut = new CreateNotificationHandler(_repository, Substitute.For<ILogger<CreateNotificationHandler>>());

    [Fact]
    public async Task Dado_DadosValidos_Quando_Handle_Entao_DevePersistirNotificacao()
    {
        var command = new CreateNotificationCommand(Guid.NewGuid(), "teste@teste.com", "Msg", NotificationType.Welcome);

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _repository.Received(1).Add(Arg.Any<Notification>());
        await _repository.Received(1).SaveChangesAsync();
    }
}
