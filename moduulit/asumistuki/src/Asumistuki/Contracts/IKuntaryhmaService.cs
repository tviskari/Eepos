using Asumistuki.Models;

namespace Asumistuki.Contracts;

/// <summary>
/// §10.3: Palauttaa kunnan kuntaryhmän (I/II/III).
/// I: Helsinki, Espoo, Kauniainen, Vantaa
/// II: 24 nimettyä kuntaa (§10.3 kohta 2)
/// III: kaikki muut
/// </summary>
public interface IKuntaryhmaService
{
    Kuntaryhma Hae(string kunta);
}
