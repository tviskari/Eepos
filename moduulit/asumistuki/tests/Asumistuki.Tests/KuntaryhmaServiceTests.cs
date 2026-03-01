using Asumistuki.Contracts;
using Asumistuki.Models;
using Asumistuki.Services;

namespace Asumistuki.Tests;

public class KuntaryhmaServiceTests
{
    private readonly IKuntaryhmaService _sut = new KuntaryhmaService();

    [Theory]
    [InlineData("Helsinki", Kuntaryhma.I)]
    [InlineData("Espoo", Kuntaryhma.I)]
    [InlineData("Kauniainen", Kuntaryhma.I)]
    [InlineData("Vantaa", Kuntaryhma.I)]
    public void Kuntaryhma_I(string kunta, Kuntaryhma odotettu)
    {
        Assert.Equal(odotettu, _sut.Hae(kunta));
    }

    [Theory]
    [InlineData("Tampere", Kuntaryhma.II)]
    [InlineData("Oulu", Kuntaryhma.II)]
    [InlineData("Turku", Kuntaryhma.II)]
    [InlineData("Jyväskylä", Kuntaryhma.II)]
    [InlineData("Kuopio", Kuntaryhma.II)]
    public void Kuntaryhma_II(string kunta, Kuntaryhma odotettu)
    {
        Assert.Equal(odotettu, _sut.Hae(kunta));
    }

    [Theory]
    [InlineData("Sodankylä", Kuntaryhma.III)]
    [InlineData("Kajaani", Kuntaryhma.III)]
    [InlineData("Mikkeli", Kuntaryhma.III)]
    public void Kuntaryhma_III(string kunta, Kuntaryhma odotettu)
    {
        Assert.Equal(odotettu, _sut.Hae(kunta));
    }

    [Fact]
    public void TuntematonKunta_PalauttaaIII()
    {
        // §10.3: "muut kuin 1 tai 2 kohdassa mainitut kunnat"
        Assert.Equal(Kuntaryhma.III, _sut.Hae("TuntematonKunta"));
    }

    [Fact]
    public void CaseInsensitive()
    {
        Assert.Equal(Kuntaryhma.I, _sut.Hae("helsinki"));
        Assert.Equal(Kuntaryhma.I, _sut.Hae("HELSINKI"));
    }
}
