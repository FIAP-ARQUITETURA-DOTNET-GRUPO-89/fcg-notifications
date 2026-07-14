using FcgNotifications.Domain.Entities;
using Shouldly;

namespace FcgNotifications.UnitTests.Domain.Entities;

public class BaseEntityTests
{
    [Fact]
    public void Dado_NovaEntidade_Quando_Criada_Entao_PossuiIdECreatedAt()
    {
        var entity = CreateTestEntity();

        entity.Id.ShouldNotBe(Guid.Empty);
        entity.CreatedAt.ShouldBeGreaterThan(DateTime.MinValue);
        entity.UpdatedAt.ShouldBeNull();
    }

    [Fact]
    public void Dado_DuasEntidadesComMesmoId_Quando_Comparadas_Entao_SaoIguais()
    {
        var entity1 = CreateTestEntity();
        var entity2 = CreateTestEntity();

        typeof(BaseEntity)
            .GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)!
            .SetValue(entity2, entity1.Id);

        entity1.Equals(entity2).ShouldBeTrue();
    }

    [Fact]
    public void Dado_EntidadesDiferentes_Quando_Comparadas_Entao_NaoSaoIguais()
    {
        var entity1 = CreateTestEntity();
        var entity2 = CreateTestEntity();

        entity1.Equals(entity2).ShouldBeFalse();
    }

    private static TestEntity CreateTestEntity() => new();
    private class TestEntity : BaseEntity { }
}
