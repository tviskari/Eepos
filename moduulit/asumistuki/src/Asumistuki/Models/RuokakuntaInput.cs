namespace Asumistuki.Models;

public record RuokakuntaInput
{
    // Ruokakunnan jäsenet
    public int Aikuiset { get; init; } = 1;      // vähintään 1 (§16.2)
    public int Lapset { get; init; }              // alle 18-v (§6)

    // Asunto
    public string Kunta { get; init; } = "";      // kunnan nimi → kuntaryhmän päättely

    // Asumismenot (§9)
    public decimal Vuokra { get; init; }          // €/kk
    public bool ErillinenLammitys { get; init; }  // lämmitys ei sisälly vuokraan
    public bool ErillinenVesi { get; init; }      // vesimaksu ei sisälly vuokraan
    public string? Maakunta { get; init; }        // lämmityskorotusta varten (§9.2)
    public decimal AlivuokralaisenVuokra { get; init; } // §9.3

    // Tulot (§12)
    public decimal BruttotulotKk { get; init; }   // ruokakunnan yhteenlasketut bruttotulot €/kk

    // Omaisuus (§13)
    public decimal? NettoOmaisuus { get; init; }  // omaisuus - velat

    // Vammaisen lisätilantarve (§11)
    public bool VammaisenLisatila { get; init; }
}
