namespace Asumistuki.Contracts;

/// <summary>
/// §13: Tulo omaisuudesta.
/// Omaisuustulo = (nettoOmaisuus − raja) × 20 % / 12
/// Raja: 1 aikuinen → 10 000 €, >1 aikuinen → 20 000 €
/// §13.5: Jos nettovarallisuus >= 50 000 € → ei oikeutta asumistukeen.
/// </summary>
public interface IOmaisuustuloLaskenta
{
    decimal Laske(decimal nettoOmaisuus, int aikuisia);
    bool OmaisuusrajaYlittyy(decimal nettoOmaisuus);
}
