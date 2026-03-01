using Asumistuki.Contracts;

namespace Asumistuki.Services;

public class PerusomavastuuLaskenta : IPerusomavastuuLaskenta
{
    public decimal Laske(decimal tulot, int aikuiset, int lapset)
    {
        // §16.2: vähintään 1 aikuinen
        if (aikuiset < 1) aikuiset = 1;

        // §52: tulot ja tuloraja pyöristetään täysiksi euroiksi
        decimal pyoristetytTulot = Math.Round(tulot, 0, MidpointRounding.AwayFromZero);

        // §16: tuloraja = 555 + 78 × aikuiset + 246 × lapset
        decimal tuloraja = 555m + 78m * aikuiset + 246m * lapset;
        tuloraja = Math.Round(tuloraja, 0, MidpointRounding.AwayFromZero);

        if (pyoristetytTulot <= tuloraja)
            return 0m;

        // §16: omavastuu = 50 % × (tulot − tuloraja)
        decimal omavastuu = 0.50m * (pyoristetytTulot - tuloraja);

        // §16.1: jos omavastuu < 10 € → 0
        if (omavastuu < 10m)
            return 0m;

        return omavastuu;
    }
}
