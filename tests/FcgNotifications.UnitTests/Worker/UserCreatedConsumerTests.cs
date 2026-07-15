using FcgNotifications.Application.Commands;
using FcgNotifications.Worker.Consumers;
using FgcGames.EventContracts.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
namespace FcgNotifications.UnitTests.Worker;

public class UserCreatedConsumerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly ILogger<UserCreatedConsumer> _logger = Substitute.For<ILogger<UserCreatedConsumer>>();
    private readonly UserCreatedConsumer _sut;

    public UserCreatedConsumerTests()
    {
        _sut = new UserCreatedConsumer(_mediator, _logger);
    }

    [Fact]
    public async Task Dado_EventoDeUsuarioCriado_Quando_Consumir_Entao_DeveEnviarComandosAoMediator()
    {
        // Arrange
        var context = Substitute.For<ConsumeContext<UserCreatedEvent>>();

        var @event = new UserCreatedEvent(Guid.NewGuid(), "Camila", "camila@teste.com", DateTime.UtcNow);
        context.Message.Returns(@event);

        // Act
        await _sut.Consume(context);

        // Assert
        await _mediator.Received(1).Send(Arg.Any<CreateUserCommand>(), Arg.Any<CancellationToken>());
        await _mediator.Received(1).Send(Arg.Any<CreateNotificationCommand>(), Arg.Any<CancellationToken>());
    }
}
