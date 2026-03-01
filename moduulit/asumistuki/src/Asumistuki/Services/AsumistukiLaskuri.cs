using Asumistuki.Contracts;
using Asumistuki.Models;

namespace Asumistuki.Services;

public class AsumistukiLaskuri : IAsumistukiLaskuri
{
    private readonly IAsumismenotLaskenta _asumismenot;
    private readonly IPerusomavastuuLaskenta _perusomavastuu;
    private readonly IOmaisuustuloLaskenta _omaisuustulo;
    private readonly IKuntaryhmaService _kuntaryhma;

    public AsumistukiLaskuri(
        IAsumismenotLaskenta asumismenot,
        IPerusomavastuuLaskenta perusomavastuu,
        IOmaisuustuloLaskenta omaisuustulo,
        IKuntaryhmaService kuntaryhma)
    {
        _asumismenot = asumismenot;
        _perusomavastuu = perusomavastuu;
        _omaisuustulo = omaisuustulo;
        _kuntaryhma = kuntaryhma;
    }

    public AsumistukiTulos Laske(RuokakuntaInput input)
    {
        int aikuiset = Math.Max(input.Aikuiset, 1);

        // 1. Omaisuustarkistus (§13.5)
        if (input.NettoOmaisuus.HasValue && _omaisuustulo.OmaisuusrajaYlittyy(input.NettoOmaisuus.Value))
        {
            return new AsumistukiTulos
            {
                TuenMaara = 0m,
                Maksetaanko = false,
                HylkaysPeruste = "Omaisuus >= 50 000 € (§13.5)"
            };
        }

        // 2. Omaisuustulo lisätään bruttotuloihin (§13)
        decimal tulot = input.BruttotulotKk;
        if (input.NettoOmaisuus.HasValue)
        {
            tulot += _omaisuustulo.Laske(input.NettoOmaisuus.Value, aikuiset);
        }

        // 3. Hyväksyttävät asumismenot (§9)
        decimal hyvaksytytMenot = _asumismenot.LaskeHyvaksytytMenot(input);

        // 4. Enimmäisasumismenot (§10, §11)
        Kuntaryhma ryhma = _kuntaryhma.Hae(input.Kunta);
        int henkilomaara = aikuiset + input.Lapset;
        if (input.VammaisenLisatila)
            henkilomaara += 1; // §11: käytetään henkilölukua + 1
        decimal enimmaisMenot = _asumismenot.HaeEnimmaisMenot(henkilomaara, ryhma);

        // 5. Huomioitavat menot = min(hyväksytyt, enimmäis)
        decimal huomioitavatMenot = Math.Min(hyvaksytytMenot, enimmaisMenot);

        // 6. Perusomavastuuosuus (§16)
        decimal perusomavastuu = _perusomavastuu.Laske(tulot, aikuiset, input.Lapset);

        // 7. Tuki = 0.70 × max(0, huomioitavat − omavastuu), pyöristys sentteihin (§52)
        decimal tuki = 0.70m * Math.Max(0m, huomioitavatMenot - perusomavastuu);
        tuki = Math.Round(tuki, 2, MidpointRounding.AwayFromZero);

        // 8. Maksetaanko = tuki >= 15 (§24)
        bool maksetaanko = tuki >= 15m;
        if (!maksetaanko)
            tuki = 0m;

        return new AsumistukiTulos
        {
            HyvaksytytMenot = hyvaksytytMenot,
            EnimmaisMenot = enimmaisMenot,
            HuomioitavatMenot = huomioitavatMenot,
            Perusomavastuuosuus = perusomavastuu,
            TuenMaara = tuki,
            Maksetaanko = maksetaanko
        };
    }
}
