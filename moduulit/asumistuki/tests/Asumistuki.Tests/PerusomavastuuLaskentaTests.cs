using Asumistuki.Contracts;
using Asumistuki.Services;

namespace Asumistuki.Tests;

public class PerusomavastuuLaskentaTests
{
    private readonly IPerusomavastuuLaskenta _sut = new PerusomavastuuLaskenta();

    [Fact]
    public void TulotAlleTulorajan_OmavastuuNolla()
    {
        // §16: tuloraja 1 aikuinen = 555 + 78 = 633
        // tulot 500 < 633 → omavastuu 0
        Assert.Equal(0m, _sut.Laske(500m, aikuiset: 1, lapset: 0));
    }

    [Fact]
    public void TulotTasanTulorajalla_OmavastuuNolla()
    {
        // §16: tuloraja = 633, tulot = 633 → omavastuu 0
        Assert.Equal(0m, _sut.Laske(633m, aikuiset: 1, lapset: 0));
    }

    [Fact]
    public void TulotYliTulorajan_Peruslaskenta()
    {
        // §16: tuloraja 1 aikuinen = 633
        // tulot 2000 → omavastuu = 0.50 × (2000 - 633) = 683.50
        Assert.Equal(683.50m, _sut.Laske(2000m, aikuiset: 1, lapset: 0));
    }

    [Fact]
    public void Pariskunta_Tuloraja()
    {
        // §16: tuloraja 2 aikuista = 555 + 78×2 = 711
        // tulot 2000 → omavastuu = 0.50 × (2000 - 711) = 644.50
        Assert.Equal(644.50m, _sut.Laske(2000m, aikuiset: 2, lapset: 0));
    }

    [Fact]
    public void LapsetKorottavatTulorajaa()
    {
        // §16: tuloraja 1 aikuinen + 2 lasta = 555 + 78 + 246×2 = 1125
        // tulot 800 < 1125 → omavastuu 0
        Assert.Equal(0m, _sut.Laske(800m, aikuiset: 1, lapset: 2));
    }

    [Fact]
    public void OmavastuuAlle10_Nollataan()
    {
        // §16.1: jos omavastuu < 10 € → 0
        // tuloraja = 633, tulot = 650 → omavastuu = 0.50 × 17 = 8.50 < 10 → 0
        Assert.Equal(0m, _sut.Laske(650m, aikuiset: 1, lapset: 0));
    }

    [Fact]
    public void OmavastuuTasan10_Huomioidaan()
    {
        // §16.1: "vähemmän kuin 10 euroa" → tasan 10 huomioidaan
        // tuloraja = 633, tarvitaan tulot = 633 + 20 = 653 → omavastuu = 0.50 × 20 = 10.00
        Assert.Equal(10.00m, _sut.Laske(653m, aikuiset: 1, lapset: 0));
    }

    [Fact]
    public void VahintaanYksiAikuinen()
    {
        // §16.2: "vähintään yksi aikuinen"
        // aikuiset = 0 → käsitellään kuin 1
        // tuloraja = 633, tulot 2000 → omavastuu = 683.50
        Assert.Equal(683.50m, _sut.Laske(2000m, aikuiset: 0, lapset: 0));
    }
}
