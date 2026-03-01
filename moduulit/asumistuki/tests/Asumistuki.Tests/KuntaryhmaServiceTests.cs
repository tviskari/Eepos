using Eepos.Kunnat;

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

    [Theory]
    [InlineData("Helsinki", "Uusimaa")]
    [InlineData("Sodankylä", "Lappi")]
    [InlineData("Kuopio", "Pohjois-Savo")]
    [InlineData("Kajaani", "Kainuu")]
    [InlineData("Mikkeli", "Etelä-Savo")]
    [InlineData("Joensuu", "Pohjois-Karjala")]
    [InlineData("Oulu", "Pohjois-Pohjanmaa")]
    [InlineData("Maarianhamina", "Ahvenanmaa")]
    public void HaeMaakunta_PalauttaaMaakunnan(string kunta, string odotettu)
    {
        // §9.2: kunnan maakunta lämmityskorotusta varten
        Assert.Equal(odotettu, _sut.HaeMaakunta(kunta));
    }

    [Fact]
    public void HaeMaakunta_TuntematonKunta_PalauttaaNull()
    {
        Assert.Null(_sut.HaeMaakunta("TuntematonKunta"));
    }

    [Fact]
    public void HaeMaakunta_CaseInsensitive()
    {
        Assert.Equal("Uusimaa", _sut.HaeMaakunta("helsinki"));
        Assert.Equal("Lappi", _sut.HaeMaakunta("SODANKYLÄ"));
    }

    [Fact]
    public void HaeKaikkiKunnat_PalauttaaKaikkiKunnat()
    {
        // Speksin kuntalistan mukaan 307 kuntaa
        var kunnat = _sut.HaeKaikkiKunnat();
        Assert.Equal(307, kunnat.Count);
    }

    [Fact]
    public void HaeKaikkiKunnat_SisaltaaHelsinginJaSodankylan()
    {
        var kunnat = _sut.HaeKaikkiKunnat();
        Assert.Contains("Helsinki", kunnat);
        Assert.Contains("Sodankylä", kunnat);
        Assert.Contains("Maarianhamina", kunnat);
    }
}
