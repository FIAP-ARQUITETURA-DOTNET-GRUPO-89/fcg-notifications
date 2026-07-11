using FcgUsers.Domain.ValueObjects;
using Shouldly;

namespace FcgNotifications.UnitTests.Domain.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Dado_EmailValido_Quando_Criar_Entao_NormalizaEAtribui()
    {
        // Arrange
        var address = " TESTE@Exemplo.com ";

        // Act
        var email = Email.Create(address);

        // Assert
        email.Address.ShouldBe("teste@exemplo.com");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("sem-arroba")]
    [InlineData("@invalido.com")]
    [InlineData("invalido@")]
    public void Dado_EmailInvalido_Quando_Criar_Entao_LancaExcecao(string address)
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => Email.Create(address));
    }
}
