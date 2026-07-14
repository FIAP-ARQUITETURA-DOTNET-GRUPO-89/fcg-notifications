//using FcgNotifications.Application.Handlers;
//using FcgNotifications.Domain.Entities;
//using FcgNotifications.Domain.Events;
//using FcgNotifications.Domain.Repositories;
//using Microsoft.Extensions.Logging;
//using NSubstitute;

//public class PaymentProcessedHandlerTests
//{
//    private readonly INotificationRepository _repository = Substitute.For<INotificationRepository>();
//    private readonly PaymentProcessedHandler _sut;

//    public PaymentProcessedHandlerTests() => _sut = new PaymentProcessedHandler(_repository, Substitute.For<ILogger<PaymentProcessedHandler>>());

//    [Fact]
//    public async Task Dado_StatusNaoAprovado_Quando_Handle_Entao_NaoDevePersistir()
//    {
//        var @event = new PaymentProcessedEvent(Guid.NewGuid(), Guid.NewGuid(), "a@a.com", "Name", 10, "Rejected");

//        await _sut.Handle(@event, CancellationToken.None);

//        _repository.DidNotReceive().Add(Arg.Any<Notification>());
//    }

//    [Fact]
//    public async Task Dado_StatusAprovado_Quando_Handle_Entao_DeveSalvarESetarComoEnviado()
//    {
//        var @event = new PaymentProcessedEvent(Guid.NewGuid(), Guid.NewGuid(), "a@a.com", "Name", 10, "Approved");

//        await _sut.Handle(@event, CancellationToken.None);

//        _repository.Received(1).Add(Arg.Any<Notification>());
//        await _repository.Received(2).SaveChangesAsync(); // Uma para Add, outra para MarkAsSent
//    }
//}
