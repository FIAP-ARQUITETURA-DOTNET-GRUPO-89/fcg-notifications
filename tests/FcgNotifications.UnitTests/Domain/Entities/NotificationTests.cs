using FcgNotifications.Domain.Entities;
using FcgNotifications.Domain.Enums;
using FcgUsers.Domain.ValueObjects;
using Shouldly;

public class NotificationTests
{
    [Fact]
    public void Dado_DadosValidos_Quando_CriarNotificacao_Entao_StatusDeveSerPending()
    {
        var email = Email.Create("teste@teste.com");
        var notification = new Notification(Guid.NewGuid(), email, "Msg", NotificationType.Welcome);

        notification.Status.ShouldBe(NotificationStatus.Pending);
    }

    [Fact]
    public void Dado_NotificacaoPendente_Quando_MarcarComoEnviada_Entao_StatusDeveSerSent()
    {
        var email = Email.Create("teste@teste.com");
        var notification = new Notification(Guid.NewGuid(), email, "Msg", NotificationType.Welcome);

        notification.MarkAsSent();

        notification.Status.ShouldBe(NotificationStatus.Sent);
    }

    [Fact]
    public void Dado_NotificacaoJaEnviada_Quando_MarcarComoEnviadaNovamente_Entao_LancaInvalidOperationException()
    {
        var email = Email.Create("teste@teste.com");
        var notification = new Notification(Guid.NewGuid(), email, "Msg", NotificationType.Welcome);
        notification.MarkAsSent();

        Should.Throw<InvalidOperationException>(() => notification.MarkAsSent());
    }
}
