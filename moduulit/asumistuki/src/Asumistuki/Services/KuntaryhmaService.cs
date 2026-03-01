using Asumistuki.Contracts;
using Asumistuki.Models;

namespace Asumistuki.Services;

public class KuntaryhmaService : IKuntaryhmaService
{
    private static readonly HashSet<string> RyhmaI = new(StringComparer.OrdinalIgnoreCase)
    {
        "Helsinki", "Espoo", "Kauniainen", "Vantaa"
    };

    private static readonly HashSet<string> RyhmaII = new(StringComparer.OrdinalIgnoreCase)
    {
        "Hyvinkää", "Hämeenlinna", "Joensuu", "Jyväskylä", "Järvenpää", "Kerava",
        "Kirkkonummi", "Kuopio", "Lahti", "Lohja", "Nokia", "Nurmijärvi", "Oulu",
        "Porvoo", "Raisio", "Riihimäki", "Rovaniemi", "Seinäjoki", "Sipoo",
        "Siuntio", "Tampere", "Turku", "Tuusula", "Vihti"
    };

    public Kuntaryhma Hae(string kunta)
    {
        if (RyhmaI.Contains(kunta)) return Kuntaryhma.I;
        if (RyhmaII.Contains(kunta)) return Kuntaryhma.II;
        return Kuntaryhma.III;
    }
}
