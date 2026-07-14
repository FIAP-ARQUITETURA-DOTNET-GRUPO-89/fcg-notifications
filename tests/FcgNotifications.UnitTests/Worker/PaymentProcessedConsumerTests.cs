//using FcgNotifications.Infrastructure.Database;
//using FcgNotifications.Worker.Consumers;
//using MassTransit;
//using MediatR;
//using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using NSubstitute;
//using Shouldly;

//namespace FcgNotifications.UnitTests.Worker;

//public class PaymentProcessedConsumerTests : IDisposable
//{
//    private readonly IMediator _mediator;
//    private readonly FcgNotificationsDbContext _dbContext;
//    private readonly SqliteConnection _connection;
//    private readonly ILogger<PaymentProcessedConsumer> _logger;
//    private readonly PaymentProcessedConsumer _sut;

//    public PaymentProcessedConsumerTests()
//    {
//        _mediator = Substitute.For<IMediator>();
//        _logger = Substitute.For<ILogger<PaymentProcessedConsumer>>();

//        _connection = new SqliteConnection("DataSource=:memory:");
//        _connection.Open();

//        var options = new DbContextOptionsBuilder<FcgNotificationsDbContext>()
//            .UseSqlite(_connection)
//            .Options;

//        _dbContext = new FcgNotificationsDbContext(options);
//        _dbContext.Database.EnsureCreated();

//        _sut = new PaymentProcessedConsumer(_mediator, _dbContext, _logger);
//    }

//    [Fact]
//    public async Task Dado_EventoPagamento_Quando_Consumir_Entao_SalvaNotificacaoComEmailDoCliente()
//    {
//        // Arrange
//        var context = Substitute.For<ConsumeContext<PaymentProcessedEvent>>();
//        var @event = new PaymentProcessedEvent(
//            OrderId: Guid.NewGuid(),
//            UserId: Guid.NewGuid(),
//            CustomerEmail: "cliente@teste.com",
//            CustomerName: "Cliente",
//            TotalAmount: 100.00m,
//            Status: "Approved");

//        context.Message.Returns(@event);

//        // Act
//        await _sut.Consume(context);

//        // Assert
//        var notification = await _dbContext.Notifications.FirstOrDefaultAsync();
//        notification.ShouldNotBeNull();
//        notification.UserEmail.Address.ShouldBe(@event.CustomerEmail);

//        // Correção: Use Arg.Any<CancellationToken>() para evitar erro de instâncias diferentes
//        await _mediator.Received(1)
//            .Publish(Arg.Any<PaymentProcessedEvent>(), Arg.Any<CancellationToken>());
//    }

//    public void Dispose()
//    {
//        _connection.Close();
//        _connection.Dispose();
//    }
//}
