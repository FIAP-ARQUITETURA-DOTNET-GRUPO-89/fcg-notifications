using FcgNotifications.Domain.Entities;
using FcgUsers.Domain.ValueObjects;
using Shouldly;

namespace FcgNotifications.UnitTests.Domain.Entities;

public class UserTests
{
    [Fact]
    public void Dado_DadosValidos_Quando_CriarUsuario_Entao_DeveAtribuirPropriedadesCorretamente()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Camila";
        var email = Email.Create("camila@test.com");

        // Act
        var user = new User(id, name, email);

        // Assert
        user.Id.ShouldBe(id);
        user.Name.ShouldBe(name);
        user.Email.ShouldBe(email);
        user.CreatedAt.ShouldBeGreaterThan(DateTime.MinValue);
    }

    [Fact]
    public void Dado_NomeVazio_Quando_CriarUsuario_Entao_DeveFuncionarMasManterVazio()
    {
        var user = new User(string.Empty, Email.Create("test@test.com"));

        user.Name.ShouldBeEmpty();
    }
}
