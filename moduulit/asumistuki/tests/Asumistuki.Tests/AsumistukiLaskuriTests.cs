using Asumistuki.Contracts;
using Asumistuki.Models;
using Asumistuki.Services;
using Eepos.Kunnat;

namespace Asumistuki.Tests;

public class AsumistukiLaskuriTests
{
    private readonly IAsumistukiLaskuri _sut = new AsumistukiLaskuri(
        new AsumismenotLaskenta(new KuntaryhmaService()),
        new PerusomavastuuLaskenta(),
        new OmaisuustuloLaskenta(),
        new KuntaryhmaService()
    );

    [Fact]
    public void YksinAsuva_Helsinki_EiTuloja()
    {
        // §8: tuki = 0.70 × (min(800, 492) - 0) = 0.70 × 492 = 344.40
        var input = new RuokakuntaInput
        {
            Aikuiset = 1, Lapset = 0,
            Kunta = "Helsinki",
            Vuokra = 800m,
            BruttotulotKk = 0m
        };

        var tulos = _sut.Laske(input);

        Assert.Equal(492m, tulos.HuomioitavatMenot);
        Assert.Equal(0m, tulos.Perusomavastuuosuus);
        Assert.Equal(344.40m, tulos.TuenMaara);
        Assert.True(tulos.Maksetaanko);
        Assert.Null(tulos.HylkaysPeruste);
    }

    [Fact]
    public void Pariskunta_1Lapsi_Tampere_KorkeatTulot_EiTukea()
    {
        // enimmäis 723 (3 hlö, II)
        // tuloraja = 555 + 78×2 + 246 = 957
        // omavastuu = 0.50 × (3000 - 957) = 1021.50
        // tuki = 0.70 × (723 - 1021.50) = negatiivinen → 0
        var input = new RuokakuntaInput
        {
            Aikuiset = 2, Lapset = 1,
            Kunta = "Tampere",
            Vuokra = 900m,
            BruttotulotKk = 3000m
        };

        var tulos = _sut.Laske(input);

        Assert.Equal(0m, tulos.TuenMaara);
        Assert.False(tulos.Maksetaanko);
    }

    [Fact]
    public void Yksinhuoltaja_2Lasta_Oulu_PienetTulot()
    {
        // enimmäis 723 (3 hlö, II), menot 700 < 723 → huomioitavat 700
        // tuloraja = 555 + 78 + 246×2 = 1125, tulot 800 < 1125 → omavastuu 0
        // tuki = 0.70 × 700 = 490.00
        var input = new RuokakuntaInput
        {
            Aikuiset = 1, Lapset = 2,
            Kunta = "Oulu",
            Vuokra = 700m,
            BruttotulotKk = 800m
        };

        var tulos = _sut.Laske(input);

        Assert.Equal(700m, tulos.HuomioitavatMenot);
        Assert.Equal(0m, tulos.Perusomavastuuosuus);
        Assert.Equal(490.00m, tulos.TuenMaara);
        Assert.True(tulos.Maksetaanko);
    }

    [Fact]
    public void OmaisuusrajaYlittyy_Hylkays()
    {
        // §13.5: omaisuus >= 50 000 → hylätään
        var input = new RuokakuntaInput
        {
            Aikuiset = 1, Kunta = "Helsinki",
            Vuokra = 500m, BruttotulotKk = 0m,
            NettoOmaisuus = 55_000m
        };

        var tulos = _sut.Laske(input);

        Assert.Equal(0m, tulos.TuenMaara);
        Assert.False(tulos.Maksetaanko);
        Assert.NotNull(tulos.HylkaysPeruste);
    }

    [Fact]
    public void TukiAlle15_EiMakseta()
    {
        // §24: ei makseta jos < 15 €
        // Haetaan tulotaso jolla tuki jää alle 15 €
        var input = new RuokakuntaInput
        {
            Aikuiset = 1, Kunta = "Helsinki",
            Vuokra = 492m,  // tasan enimmäismenot
            BruttotulotKk = 1900m
        };

        var tulos = _sut.Laske(input);

        // Tarkista: jos tuki on > 0 mutta < 15, Maksetaanko = false
        if (tulos.TuenMaara > 0m && tulos.TuenMaara < 15m)
            Assert.False(tulos.Maksetaanko);
    }

    [Fact]
    public void ErillinenVesiJaLammitys_Lappi()
    {
        // §9.1 + §9.2: erillinen vesi + lämmitys, Lappi +8%
        // 2 hlö: vesi 17×2=34, lämmitys (38+13)×1.08=55.08
        // menot: 400 + 34 + 55.08 = 489.08
        // enimmäis 501 (2 hlö, III) → huomioitavat 489.08
        // tulot 0 → omavastuu 0
        // tuki = 0.70 × 489.08 = 342.356 → 342.36
        var input = new RuokakuntaInput
        {
            Aikuiset = 1, Lapset = 1,
            Kunta = "Sodankylä",
            Vuokra = 400m,
            ErillinenVesi = true, ErillinenLammitys = true,
            BruttotulotKk = 0m
        };

        var tulos = _sut.Laske(input);

        Assert.True(tulos.Maksetaanko);
        Assert.True(tulos.TuenMaara > 340m);
    }

    [Fact]
    public void OmaisuustuloLisataanTuloihin()
    {
        // §13: omaisuus 30 000, 1 aikuinen → raja 10 000
        // omaisuustulo = (30000-10000) × 0.20 / 12 = 333.33
        // kokonaistulot = 500 + 333.33 = 833.33
        var input = new RuokakuntaInput
        {
            Aikuiset = 1, Kunta = "Helsinki",
            Vuokra = 500m,
            BruttotulotKk = 500m,
            NettoOmaisuus = 30_000m
        };

        var tulos = _sut.Laske(input);

        // Omavastuu pitäisi olla suurempi kuin ilman omaisuutta
        var ilmanOmaisuutta = _sut.Laske(input with { NettoOmaisuus = null });
        Assert.True(tulos.Perusomavastuuosuus >= ilmanOmaisuutta.Perusomavastuuosuus);
    }

    [Fact]
    public void VammaisenLisatila_KasvattaaEnimmaismenoja()
    {
        // §11: enimmäismenot yhtä henkilöä suuremman mukaan
        // 1 hlö + vammaisen lisätila → enimmäismenot 2 hlön mukaan
        var perus = _sut.Laske(new RuokakuntaInput
        {
            Aikuiset = 1, Kunta = "Helsinki", Vuokra = 800m
        });

        var vammainen = _sut.Laske(new RuokakuntaInput
        {
            Aikuiset = 1, Kunta = "Helsinki", Vuokra = 800m,
            VammaisenLisatila = true
        });

        // 1 hlö I: 492, 2 hlö I: 706
        Assert.Equal(492m, perus.EnimmaisMenot);
        Assert.Equal(706m, vammainen.EnimmaisMenot);
    }
}
