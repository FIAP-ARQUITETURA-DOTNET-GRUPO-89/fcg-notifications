//using FcgNotifications.Application.Handlers;
//using FcgNotifications.Domain.Entities;
//using FcgNotifications.Domain.Events;
//using FcgNotifications.Domain.Repositories;
//using Microsoft.Extensions.Logging;
//using NSubstitute;
//using Shouldly;
//public class UserCreatedHandlerTests
//{
//    private readonly INotificationRepository _repository = Substitute.For<INotificationRepository>();
//    private readonly UserCreatedHandler _sut;

//    public UserCreatedHandlerTests() => _sut = new UserCreatedHandler(_repository, Substitute.For<ILogger<UserCreatedHandler>>());

//    [Fact]
//    public async Task Dado_ErroNoEnvio_Quando_Handle_Entao_DeveMarcarComoFailedEPropagarExcecao()
//    {
//        // Arrange
//        var @event = new UserCreatedEvent(Guid.NewGuid(), "Nome", "a@a.com", DateTime.UtcNow);
//        var repo = Substitute.For<INotificationRepository>();
//        var sut = new UserCreatedHandler(repo, Substitute.For<ILogger<UserCreatedHandler>>());

//        repo.When(x => x.SaveChangesAsync()).Do(x => throw new Exception("Erro"));

//        // Act & Assert
//        await Should.ThrowAsync<Exception>(() => sut.Handle(@event, CancellationToken.None));

//        repo.Received().Add(Arg.Any<Notification>());
//    }
//}
