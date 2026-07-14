using FcgNotifications.Domain.Events;
using FcgNotifications.Infrastructure.Database;
using FcgNotifications.Worker.Consumers;
using MassTransit;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;

namespace FcgNotifications.UnitTests.Worker;

public class UserCreatedConsumerTests : IDisposable
{
    private readonly IMediator _mediator;
    private readonly FcgNotificationsDbContext _dbContext;
    private readonly SqliteConnection _connection;
    private readonly ILogger<UserCreatedConsumer> _logger;
    private readonly UserCreatedConsumer _sut;

    public UserCreatedConsumerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _logger = Substitute.For<ILogger<UserCreatedConsumer>>();

        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<FcgNotificationsDbContext>()
            .UseSqlite(_connection)
            .Options;

        _dbContext = new FcgNotificationsDbContext(options);
        _dbContext.Database.EnsureCreated();

        _sut = new UserCreatedConsumer(_mediator, _dbContext, _logger);
    }

    [Fact]
    public async Task Dado_EventoValido_Quando_Consumir_Entao_SalvaNotificacaoEPublishMediatR()
    {
        // Arrange
        var context = Substitute.For<ConsumeContext<UserCreatedEvent>>();
        var @event = new UserCreatedEvent(Guid.NewGuid(), "Teste", "teste@teste.com", DateTime.UtcNow);
        context.Message.Returns(@event);

        // Act
        await _sut.Consume(context);

        // Assert
        var notification = await _dbContext.Notifications.FirstOrDefaultAsync();
        notification.ShouldNotBeNull();
        notification.UserEmail.Address.ShouldBe("teste@teste.com");

        await _mediator.Received(1)
            .Publish(Arg.Any<UserCreatedEvent>(), Arg.Any<CancellationToken>());
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}
