using Asumistuki.Models;

namespace Asumistuki.Contracts;

/// <summary>
/// §8: Päälaskentafunktio.
/// tuki = 0.70 × (min(hyväksytytMenot, enimmäisMenot) − perusomavastuuosuus)
/// Hylkäysperusteet: omaisuusraja (§13.5)
/// Ei makseta jos tuki &lt; 15 € (§24)
/// </summary>
public interface IAsumistukiLaskuri
{
    AsumistukiTulos Laske(RuokakuntaInput input);
}
