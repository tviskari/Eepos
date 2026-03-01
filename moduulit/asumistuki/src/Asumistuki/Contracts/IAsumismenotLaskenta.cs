using Asumistuki.Models;
using Eepos.Kunnat;

namespace Asumistuki.Contracts;

/// <summary>
/// §9: Hyväksyttävien asumismenojen laskenta.
/// §10: Enimmäisasumismenojen haku kuntaryhmän ja henkilömäärän mukaan.
/// §11: Vammaisen lisätilantarve.
/// </summary>
public interface IAsumismenotLaskenta
{
    /// <summary>
    /// §9: Laskee hyväksyttävät asumismenot.
    /// Vuokra + erikseen maksettava vesi (17 €/hlö/kk) + lämmitys (38 € + 13 €/lisählö)
    /// - maakuntakorotukset (§9.2): E-Savo/P-Savo/P-Karjala +4%, P-Pohjanmaa/Kainuu/Lappi +8%
    /// - alivuokralaisen vuokra vähennetään (§9.3)
    /// </summary>
    decimal LaskeHyvaksytytMenot(RuokakuntaInput input);

    /// <summary>
    /// §10: Palauttaa enimmäisasumismenot kuntaryhmän ja henkilömäärän mukaan.
    /// Taulukko 1-4 hlö + korotus jokaisesta lisähenkilöstä (>4).
    /// §11: Jos vammaisenLisatila, käytetään henkilölukua + 1.
    /// </summary>
    decimal HaeEnimmaisMenot(int henkilomaara, Kuntaryhma ryhma);
}
