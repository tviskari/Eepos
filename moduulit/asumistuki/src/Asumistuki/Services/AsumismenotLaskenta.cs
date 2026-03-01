using Asumistuki.Contracts;
using Asumistuki.Models;

namespace Asumistuki.Services;

public class AsumismenotLaskenta : IAsumismenotLaskenta
{
    // §10.1: Enimmäisasumismenot (kuntaryhmä, henkilömäärä) → €/kk
    private static readonly Dictionary<(Kuntaryhma, int), decimal> Enimmaismenot = new()
    {
        { (Kuntaryhma.I, 1), 492m },
        { (Kuntaryhma.I, 2), 706m },
        { (Kuntaryhma.I, 3), 890m },
        { (Kuntaryhma.I, 4), 1038m },

        { (Kuntaryhma.II, 1), 390m },
        { (Kuntaryhma.II, 2), 561m },
        { (Kuntaryhma.II, 3), 723m },
        { (Kuntaryhma.II, 4), 856m },

        { (Kuntaryhma.III, 1), 344m },
        { (Kuntaryhma.III, 2), 501m },
        { (Kuntaryhma.III, 3), 645m },
        { (Kuntaryhma.III, 4), 764m },
    };

    // §10.2: Lisähenkilökorotus (>4 hlö) per kuntaryhmä
    private static readonly Dictionary<Kuntaryhma, decimal> Lisakorotus = new()
    {
        { Kuntaryhma.I, 130m },
        { Kuntaryhma.II, 117m },
        { Kuntaryhma.III, 112m },
    };

    // §9.2: Maakuntakorotus lämmitykseen
    private static readonly HashSet<string> Korotus4 = new(StringComparer.OrdinalIgnoreCase)
    {
        "Etelä-Savo", "Pohjois-Savo", "Pohjois-Karjala"
    };

    private static readonly HashSet<string> Korotus8 = new(StringComparer.OrdinalIgnoreCase)
    {
        "Pohjois-Pohjanmaa", "Kainuu", "Lappi"
    };

    public decimal LaskeHyvaksytytMenot(RuokakuntaInput input)
    {
        int henkilomaara = input.Aikuiset + input.Lapset;
        if (henkilomaara < 1) henkilomaara = 1;

        decimal menot = input.Vuokra;

        // §9.1: Erillinen vesimaksu 17 €/hlö/kk
        if (input.ErillinenVesi)
        {
            menot += 17m * henkilomaara;
        }

        // §9.1: Erillinen lämmitys = 38 + 13 × (hlömäärä - 1)
        if (input.ErillinenLammitys)
        {
            decimal lammitys = 38m + 13m * (henkilomaara - 1);

            // §9.2: Maakuntakorotus
            if (input.Maakunta is not null)
            {
                if (Korotus8.Contains(input.Maakunta))
                    lammitys *= 1.08m;
                else if (Korotus4.Contains(input.Maakunta))
                    lammitys *= 1.04m;
            }

            menot += lammitys;
        }

        // §9.3: Alivuokralaisen vuokra vähennetään
        menot -= input.AlivuokralaisenVuokra;

        return menot;
    }

    public decimal HaeEnimmaisMenot(int henkilomaara, Kuntaryhma ryhma)
    {
        if (henkilomaara < 1) henkilomaara = 1;

        if (henkilomaara <= 4)
        {
            return Enimmaismenot[(ryhma, henkilomaara)];
        }

        // §10.2: Yli 4 henkilöä
        decimal perus = Enimmaismenot[(ryhma, 4)];
        decimal korotus = Lisakorotus[ryhma];
        return perus + korotus * (henkilomaara - 4);
    }
}
