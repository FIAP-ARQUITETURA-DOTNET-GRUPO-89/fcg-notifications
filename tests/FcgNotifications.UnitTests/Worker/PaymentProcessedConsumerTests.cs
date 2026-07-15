using FcgNotifications.Application.Commands;
using FcgNotifications.Worker.Consumers;
using FgcGames.EventContracts.Events;
using FgcGames.EventContracts.Enums;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FcgNotifications.UnitTests.Worker;

public class PaymentProcessedConsumerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly ILogger<PaymentProcessedConsumer> _logger = Substitute.For<ILogger<PaymentProcessedConsumer>>();
    private readonly PaymentProcessedConsumer _sut;

    public PaymentProcessedConsumerTests()
    {
        _sut = new PaymentProcessedConsumer(_mediator, _logger);
    }

    [Fact]
    public async Task Dado_EventoDePagamentoAprovado_Quando_Consumir_Entao_DeveEnviarComandoAoMediator()
    {
        // Arrange
        var context = Substitute.For<ConsumeContext<PaymentProcessedEvent>>();

        var @event = new PaymentProcessedEvent(
            OrderId: Guid.NewGuid(),
            UserId: Guid.NewGuid(),
            GameId: Guid.NewGuid(),
            Price: 100.0m,
            Status: PaymentStatus.Approved,
            ProcessedAtUtc: DateTime.UtcNow);

        context.Message.Returns(@event);

        // Act
        await _sut.Consume(context);

        // Assert
        await _mediator.Received(1).Send(Arg.Is<ProcessPaymentResultCommand>(c =>
            c.OrderId == @event.OrderId &&
            c.UserId == @event.UserId &&
            c.Status == PaymentStatus.Approved), Arg.Any<CancellationToken>());
    }
}
