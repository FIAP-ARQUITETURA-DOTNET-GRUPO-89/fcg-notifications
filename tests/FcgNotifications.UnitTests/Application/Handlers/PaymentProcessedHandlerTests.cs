using FcgNotifications.Application.Commands;
using FcgNotifications.Application.Handlers;
using FcgNotifications.Domain.Entities;
using FcgNotifications.Domain.Repositories;
using FcgUsers.Domain.ValueObjects;
using FgcGames.EventContracts.Enums;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;

public class PaymentProcessedHandlerTests
{
    private readonly INotificationRepository _repository = Substitute.For<INotificationRepository>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly PaymentProcessedHandler _sut;

    public PaymentProcessedHandlerTests() =>
        _sut = new PaymentProcessedHandler(_repository, _userRepository, Substitute.For<ILogger<PaymentProcessedHandler>>());

    [Fact]
    public async Task Dado_StatusNaoAprovado_Quando_Handle_Entao_DeveRetornarErro()
    {
        var command = new ProcessPaymentResultCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), PaymentStatus.Rejected);

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        _repository.DidNotReceive().Add(Arg.Any<Notification>());
    }

    [Fact]
    public async Task Dado_StatusAprovadoUsuarioExistente_Quando_Handle_Entao_DeveSalvarESetarComoEnviado()
    {
        var userId = Guid.NewGuid();
        var command = new ProcessPaymentResultCommand(Guid.NewGuid(), userId, Guid.NewGuid(), PaymentStatus.Approved);
        var user = new User(userId, "Name", Email.Create("a@a.com"));
        _userRepository.GetByIdAsync(userId).Returns(user);

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _repository.Received(1).Add(Arg.Any<Notification>());
        await _repository.Received(2).SaveChangesAsync();
    }
}
