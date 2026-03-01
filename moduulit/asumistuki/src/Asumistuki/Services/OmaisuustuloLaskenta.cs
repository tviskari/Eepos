using Asumistuki.Contracts;

namespace Asumistuki.Services;

public class OmaisuustuloLaskenta : IOmaisuustuloLaskenta
{
    public decimal Laske(decimal nettoOmaisuus, int aikuisia)
    {
        // §13: raja: 1 aikuinen → 10 000, >1 → 20 000
        decimal raja = aikuisia <= 1 ? 10_000m : 20_000m;

        if (nettoOmaisuus <= raja)
            return 0m;

        // §13: omaisuustulo = (nettoOmaisuus − raja) × 20 % / 12
        return (nettoOmaisuus - raja) * 0.20m / 12m;
    }

    public bool OmaisuusrajaYlittyy(decimal nettoOmaisuus)
    {
        // §13.5: >= 50 000 → ei oikeutta asumistukeen
        return nettoOmaisuus >= 50_000m;
    }
}
