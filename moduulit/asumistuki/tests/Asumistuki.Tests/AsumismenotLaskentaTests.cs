using Asumistuki.Contracts;
using Asumistuki.Models;
using Asumistuki.Services;
using Eepos.Kunnat;

namespace Asumistuki.Tests;

public class AsumismenotLaskentaTests
{
    private readonly IAsumismenotLaskenta _sut = new AsumismenotLaskenta(new KuntaryhmaService());

    [Fact]
    public void PelkkaVuokra_PalauttaaVuokran()
    {
        // §9.1: vuokra kun vesi ja lämmitys sisältyvät
        var input = new RuokakuntaInput { Vuokra = 800m, Aikuiset = 1 };
        Assert.Equal(800m, _sut.LaskeHyvaksytytMenot(input));
    }

    [Fact]
    public void ErillinenVesi_Lisataan_17EuroaPerHenkilö()
    {
        // §9.1: erikseen maksettava vesi 17 €/hlö/kk
        // 2 henkilöä: 500 + 17×2 = 534
        var input = new RuokakuntaInput
        {
            Vuokra = 500m, Aikuiset = 1, Lapset = 1,
            ErillinenVesi = true
        };
        Assert.Equal(534m, _sut.LaskeHyvaksytytMenot(input));
    }

    [Fact]
    public void ErillinenLammitys_Peruskaava()
    {
        // §9.1: lämmitys = 38 + 13 × (hlömäärä - 1)
        // 1 henkilö: 500 + 38 = 538
        var input = new RuokakuntaInput
        {
            Vuokra = 500m, Aikuiset = 1,
            ErillinenLammitys = true
        };
        Assert.Equal(538m, _sut.LaskeHyvaksytytMenot(input));
    }

    [Fact]
    public void ErillinenLammitys_3Henkea()
    {
        // §9.1: 3 hlö → lämmitys = 38 + 13×2 = 64
        // 500 + 64 = 564
        var input = new RuokakuntaInput
        {
            Vuokra = 500m, Aikuiset = 2, Lapset = 1,
            ErillinenLammitys = true
        };
        Assert.Equal(564m, _sut.LaskeHyvaksytytMenot(input));
    }

    [Fact]
    public void LammitysKorotus_Lappi_8Prosenttia()
    {
        // §9.2: Sodankylä → Lappi → lämmitys × 1.08
        // 1 hlö: lämmitys = 38 × 1.08 = 41.04
        // 500 + 41.04 = 541.04
        var input = new RuokakuntaInput
        {
            Vuokra = 500m, Aikuiset = 1, Kunta = "Sodankylä",
            ErillinenLammitys = true
        };
        Assert.Equal(541.04m, _sut.LaskeHyvaksytytMenot(input));
    }

    [Fact]
    public void LammitysKorotus_PohjoisSavo_4Prosenttia()
    {
        // §9.2: Kuopio → Pohjois-Savo → lämmitys × 1.04
        // 1 hlö: lämmitys = 38 × 1.04 = 39.52
        // 500 + 39.52 = 539.52
        var input = new RuokakuntaInput
        {
            Vuokra = 500m, Aikuiset = 1, Kunta = "Kuopio",
            ErillinenLammitys = true
        };
        Assert.Equal(539.52m, _sut.LaskeHyvaksytytMenot(input));
    }

    [Fact]
    public void AlivuokralaisenVuokra_Vahennetaan()
    {
        // §9.3: alivuokralaisen vuokra vähennetään
        // 800 - 300 = 500
        var input = new RuokakuntaInput
        {
            Vuokra = 800m, Aikuiset = 1,
            AlivuokralaisenVuokra = 300m
        };
        Assert.Equal(500m, _sut.LaskeHyvaksytytMenot(input));
    }

    [Theory]
    [InlineData(1, Kuntaryhma.I, 492)]
    [InlineData(2, Kuntaryhma.I, 706)]
    [InlineData(3, Kuntaryhma.I, 890)]
    [InlineData(4, Kuntaryhma.I, 1038)]
    [InlineData(1, Kuntaryhma.II, 390)]
    [InlineData(4, Kuntaryhma.II, 856)]
    [InlineData(1, Kuntaryhma.III, 344)]
    [InlineData(4, Kuntaryhma.III, 764)]
    public void EnimmaisMenot_Taulukko(int henkilomaara, Kuntaryhma ryhma, decimal odotettu)
    {
        // §10.1: enimmäisasumismenot taulukosta
        Assert.Equal(odotettu, _sut.HaeEnimmaisMenot(henkilomaara, ryhma));
    }

    [Theory]
    [InlineData(5, Kuntaryhma.I, 1168)]   // §10.2: 1038 + 130 = 1168
    [InlineData(6, Kuntaryhma.I, 1298)]   // §10.2: 1038 + 2×130 = 1298
    [InlineData(5, Kuntaryhma.II, 973)]   // §10.2: 856 + 117 = 973
    [InlineData(5, Kuntaryhma.III, 876)]  // §10.2: 764 + 112 = 876
    public void EnimmaisMenot_Yli4Henkea(int henkilomaara, Kuntaryhma ryhma, decimal odotettu)
    {
        // §10.2: 4 hlö perusarvo + korotus per lisähenkilö
        Assert.Equal(odotettu, _sut.HaeEnimmaisMenot(henkilomaara, ryhma));
    }
}
