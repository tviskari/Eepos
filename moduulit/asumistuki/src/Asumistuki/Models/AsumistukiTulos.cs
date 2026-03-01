namespace Asumistuki.Models;

public record AsumistukiTulos
{
    public decimal HyvaksytytMenot { get; init; }
    public decimal EnimmaisMenot { get; init; }
    public decimal HuomioitavatMenot { get; init; }  // min(hyväksytyt, enimmäis)
    public decimal Perusomavastuuosuus { get; init; }
    public decimal TuenMaara { get; init; }           // senttien tarkkuus (§52)
    public bool Maksetaanko { get; init; }            // false jos < 15 € (§24)
    public string? HylkaysPeruste { get; init; }      // esim. "Omaisuus >= 50 000 € (§13.5)"
}
