using FcgNotifications.Application.Commands;
using FcgNotifications.Worker.Consumers;
using FgcGames.EventContracts.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;

public class UserCreatedConsumerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly UserCreatedConsumer _sut;

    public UserCreatedConsumerTests() =>
        _sut = new UserCreatedConsumer(_mediator, Substitute.For<ILogger<UserCreatedConsumer>>());

    [Fact]
    public async Task Dado_EventoValido_Quando_Consumir_Entao_DeveEnviarComandosDeCriacaoDeUsuarioENotificacao()
    {
        // Arrange
        var context = Substitute.For<ConsumeContext<UserCreatedEvent>>();
        var eventData = new UserCreatedEvent(Guid.NewGuid(), "Camila", "camila@test.com", DateTime.UtcNow);
        context.Message.Returns(eventData);

        // Act
        await _sut.Consume(context);

        // Assert
        await _mediator.Received(1).Send(Arg.Any<CreateUserCommand>(), Arg.Any<CancellationToken>());
        await _mediator.Received(1).Send(Arg.Any<CreateNotificationCommand>(), Arg.Any<CancellationToken>());
    }
}
