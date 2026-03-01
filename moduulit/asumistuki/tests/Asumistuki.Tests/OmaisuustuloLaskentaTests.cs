using Asumistuki.Contracts;
using Asumistuki.Services;

namespace Asumistuki.Tests;

public class OmaisuustuloLaskentaTests
{
    private readonly IOmaisuustuloLaskenta _sut = new OmaisuustuloLaskenta();

    [Fact]
    public void OmaisuusAlleRajan_EiTuloa()
    {
        // §13: 1 aikuinen, raja 10 000, omaisuus 5 000 → tulo 0
        Assert.Equal(0m, _sut.Laske(5_000m, aikuisia: 1));
    }

    [Fact]
    public void OmaisuusYliRajan_YksiAikuinen()
    {
        // §13: raja 10 000, omaisuus 30 000 → (30000-10000) × 0.20 / 12 = 333.33...
        var tulo = _sut.Laske(30_000m, aikuisia: 1);
        Assert.True(tulo > 333m && tulo < 334m);
    }

    [Fact]
    public void OmaisuusYliRajan_KaksiAikuista()
    {
        // §13: raja 20 000, omaisuus 30 000 → (30000-20000) × 0.20 / 12 = 166.67
        var tulo = _sut.Laske(30_000m, aikuisia: 2);
        Assert.True(tulo > 166m && tulo < 167m);
    }

    [Fact]
    public void OmaisuusrajaYlittyy_50000()
    {
        // §13.5: >= 50 000 → ei oikeutta
        Assert.True(_sut.OmaisuusrajaYlittyy(50_000m));
        Assert.True(_sut.OmaisuusrajaYlittyy(100_000m));
    }

    [Fact]
    public void OmaisuusrajaEiYlity_49999()
    {
        // §13.5: < 50 000 → ok
        Assert.False(_sut.OmaisuusrajaYlittyy(49_999m));
    }
}
