namespace Asumistuki.Contracts;

/// <summary>
/// §16: Perusomavastuuosuuden laskenta.
/// Omavastuu = 50 % × (tulot − tuloraja)
/// Tuloraja = 555 + 78 × aikuiset + 246 × lapset
/// Jos omavastuu &lt; 10 € → 0
/// Vähintään 1 aikuinen (§16.2)
/// Pyöristyssäännöt (§52): tulot ja tuloraja pyöristetään täysiksi euroiksi.
/// </summary>
public interface IPerusomavastuuLaskenta
{
    decimal Laske(decimal tulot, int aikuiset, int lapset);
}
