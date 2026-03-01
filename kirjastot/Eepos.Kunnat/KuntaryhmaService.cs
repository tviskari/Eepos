using System.Globalization;

namespace Eepos.Kunnat;

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

    // §9.2: Kunta → maakunta -mappaus (307 kuntaa, 1.1.2025)
    private static readonly Dictionary<string, string> KuntaToMaakunta =
        new(StringComparer.OrdinalIgnoreCase)
    {
        // Uusimaa
        ["Askola"] = "Uusimaa", ["Espoo"] = "Uusimaa", ["Hanko"] = "Uusimaa",
        ["Helsinki"] = "Uusimaa", ["Hyvinkää"] = "Uusimaa", ["Inkoo"] = "Uusimaa",
        ["Järvenpää"] = "Uusimaa", ["Karkkila"] = "Uusimaa", ["Kauniainen"] = "Uusimaa",
        ["Kerava"] = "Uusimaa", ["Kirkkonummi"] = "Uusimaa", ["Lapinjärvi"] = "Uusimaa",
        ["Lohja"] = "Uusimaa", ["Loviisa"] = "Uusimaa", ["Myrskylä"] = "Uusimaa",
        ["Mäntsälä"] = "Uusimaa", ["Nurmijärvi"] = "Uusimaa", ["Pornainen"] = "Uusimaa",
        ["Porvoo"] = "Uusimaa", ["Pukkila"] = "Uusimaa", ["Raasepori"] = "Uusimaa",
        ["Sipoo"] = "Uusimaa", ["Siuntio"] = "Uusimaa", ["Tuusula"] = "Uusimaa",
        ["Vantaa"] = "Uusimaa", ["Vihti"] = "Uusimaa",

        // Varsinais-Suomi
        ["Aura"] = "Varsinais-Suomi", ["Kaarina"] = "Varsinais-Suomi",
        ["Kemiönsaari"] = "Varsinais-Suomi", ["Koski Tl"] = "Varsinais-Suomi",
        ["Kustavi"] = "Varsinais-Suomi", ["Laitila"] = "Varsinais-Suomi",
        ["Lieto"] = "Varsinais-Suomi", ["Loimaa"] = "Varsinais-Suomi",
        ["Marttila"] = "Varsinais-Suomi", ["Masku"] = "Varsinais-Suomi",
        ["Mynämäki"] = "Varsinais-Suomi", ["Naantali"] = "Varsinais-Suomi",
        ["Nousiainen"] = "Varsinais-Suomi", ["Oripää"] = "Varsinais-Suomi",
        ["Paimio"] = "Varsinais-Suomi", ["Parainen"] = "Varsinais-Suomi",
        ["Pyhäranta"] = "Varsinais-Suomi", ["Pöytyä"] = "Varsinais-Suomi",
        ["Raisio"] = "Varsinais-Suomi", ["Rusko"] = "Varsinais-Suomi",
        ["Salo"] = "Varsinais-Suomi", ["Sauvo"] = "Varsinais-Suomi",
        ["Somero"] = "Varsinais-Suomi", ["Taivassalo"] = "Varsinais-Suomi",
        ["Turku"] = "Varsinais-Suomi", ["Uusikaupunki"] = "Varsinais-Suomi",
        ["Vehmaa"] = "Varsinais-Suomi",

        // Satakunta
        ["Eura"] = "Satakunta", ["Eurajoki"] = "Satakunta",
        ["Harjavalta"] = "Satakunta", ["Huittinen"] = "Satakunta",
        ["Jämijärvi"] = "Satakunta", ["Kankaanpää"] = "Satakunta",
        ["Karvia"] = "Satakunta", ["Kokemäki"] = "Satakunta",
        ["Merikarvia"] = "Satakunta", ["Nakkila"] = "Satakunta",
        ["Pomarkku"] = "Satakunta", ["Pori"] = "Satakunta",
        ["Rauma"] = "Satakunta", ["Siikainen"] = "Satakunta",
        ["Säkylä"] = "Satakunta", ["Ulvila"] = "Satakunta",

        // Kanta-Häme
        ["Forssa"] = "Kanta-Häme", ["Hattula"] = "Kanta-Häme",
        ["Hausjärvi"] = "Kanta-Häme", ["Humppila"] = "Kanta-Häme",
        ["Hämeenlinna"] = "Kanta-Häme", ["Janakkala"] = "Kanta-Häme",
        ["Jokioinen"] = "Kanta-Häme", ["Loppi"] = "Kanta-Häme",
        ["Riihimäki"] = "Kanta-Häme", ["Tammela"] = "Kanta-Häme",
        ["Ypäjä"] = "Kanta-Häme",

        // Pirkanmaa
        ["Akaa"] = "Pirkanmaa", ["Hämeenkyrö"] = "Pirkanmaa",
        ["Ikaalinen"] = "Pirkanmaa", ["Juupajoki"] = "Pirkanmaa",
        ["Kangasala"] = "Pirkanmaa", ["Kihniö"] = "Pirkanmaa",
        ["Kuhmoinen"] = "Pirkanmaa", ["Lempäälä"] = "Pirkanmaa",
        ["Mänttä-Vilppula"] = "Pirkanmaa", ["Nokia"] = "Pirkanmaa",
        ["Orivesi"] = "Pirkanmaa", ["Parkano"] = "Pirkanmaa",
        ["Pirkkala"] = "Pirkanmaa", ["Pälkäne"] = "Pirkanmaa",
        ["Ruovesi"] = "Pirkanmaa", ["Sastamala"] = "Pirkanmaa",
        ["Tampere"] = "Pirkanmaa", ["Urjala"] = "Pirkanmaa",
        ["Valkeakoski"] = "Pirkanmaa", ["Vesilahti"] = "Pirkanmaa",
        ["Virrat"] = "Pirkanmaa", ["Ylöjärvi"] = "Pirkanmaa",

        // Päijät-Häme
        ["Asikkala"] = "Päijät-Häme", ["Hartola"] = "Päijät-Häme",
        ["Heinola"] = "Päijät-Häme", ["Hollola"] = "Päijät-Häme",
        ["Iitti"] = "Päijät-Häme", ["Kärkölä"] = "Päijät-Häme",
        ["Lahti"] = "Päijät-Häme", ["Orimattila"] = "Päijät-Häme",
        ["Padasjoki"] = "Päijät-Häme", ["Sysmä"] = "Päijät-Häme",

        // Kymenlaakso
        ["Hamina"] = "Kymenlaakso", ["Kotka"] = "Kymenlaakso",
        ["Kouvola"] = "Kymenlaakso", ["Miehikkälä"] = "Kymenlaakso",
        ["Pyhtää"] = "Kymenlaakso", ["Virolahti"] = "Kymenlaakso",

        // Etelä-Karjala
        ["Imatra"] = "Etelä-Karjala", ["Lappeenranta"] = "Etelä-Karjala",
        ["Lemi"] = "Etelä-Karjala", ["Luumäki"] = "Etelä-Karjala",
        ["Parikkala"] = "Etelä-Karjala", ["Rautjärvi"] = "Etelä-Karjala",
        ["Ruokolahti"] = "Etelä-Karjala", ["Savitaipale"] = "Etelä-Karjala",
        ["Taipalsaari"] = "Etelä-Karjala",

        // Etelä-Savo (+4 % lämmityskorotus)
        ["Enonkoski"] = "Etelä-Savo", ["Heinävesi"] = "Etelä-Savo",
        ["Hirvensalmi"] = "Etelä-Savo", ["Juva"] = "Etelä-Savo",
        ["Kangasniemi"] = "Etelä-Savo", ["Mikkeli"] = "Etelä-Savo",
        ["Mäntyharju"] = "Etelä-Savo", ["Pieksämäki"] = "Etelä-Savo",
        ["Puumala"] = "Etelä-Savo", ["Rantasalmi"] = "Etelä-Savo",
        ["Savonlinna"] = "Etelä-Savo", ["Sulkava"] = "Etelä-Savo",

        // Pohjois-Savo (+4 % lämmityskorotus)
        ["Iisalmi"] = "Pohjois-Savo", ["Joroinen"] = "Pohjois-Savo",
        ["Kaavi"] = "Pohjois-Savo", ["Keitele"] = "Pohjois-Savo",
        ["Kiuruvesi"] = "Pohjois-Savo", ["Kuopio"] = "Pohjois-Savo",
        ["Lapinlahti"] = "Pohjois-Savo", ["Leppävirta"] = "Pohjois-Savo",
        ["Pielavesi"] = "Pohjois-Savo", ["Rautalampi"] = "Pohjois-Savo",
        ["Rautavaara"] = "Pohjois-Savo", ["Siilinjärvi"] = "Pohjois-Savo",
        ["Sonkajärvi"] = "Pohjois-Savo", ["Suonenjoki"] = "Pohjois-Savo",
        ["Tervo"] = "Pohjois-Savo", ["Tuusniemi"] = "Pohjois-Savo",
        ["Varkaus"] = "Pohjois-Savo", ["Vesanto"] = "Pohjois-Savo",
        ["Vieremä"] = "Pohjois-Savo",

        // Pohjois-Karjala (+4 % lämmityskorotus)
        ["Ilomantsi"] = "Pohjois-Karjala", ["Joensuu"] = "Pohjois-Karjala",
        ["Juuka"] = "Pohjois-Karjala", ["Kitee"] = "Pohjois-Karjala",
        ["Kontiolahti"] = "Pohjois-Karjala", ["Lieksa"] = "Pohjois-Karjala",
        ["Liperi"] = "Pohjois-Karjala", ["Nurmes"] = "Pohjois-Karjala",
        ["Outokumpu"] = "Pohjois-Karjala", ["Polvijärvi"] = "Pohjois-Karjala",
        ["Rääkkylä"] = "Pohjois-Karjala", ["Tohmajärvi"] = "Pohjois-Karjala",

        // Keski-Suomi
        ["Hankasalmi"] = "Keski-Suomi", ["Joutsa"] = "Keski-Suomi",
        ["Jyväskylä"] = "Keski-Suomi", ["Jämsä"] = "Keski-Suomi",
        ["Kannonkoski"] = "Keski-Suomi", ["Karstula"] = "Keski-Suomi",
        ["Keuruu"] = "Keski-Suomi", ["Kinnula"] = "Keski-Suomi",
        ["Kivijärvi"] = "Keski-Suomi", ["Konnevesi"] = "Keski-Suomi",
        ["Kyyjärvi"] = "Keski-Suomi", ["Laukaa"] = "Keski-Suomi",
        ["Luhanka"] = "Keski-Suomi", ["Multia"] = "Keski-Suomi",
        ["Muurame"] = "Keski-Suomi", ["Petäjävesi"] = "Keski-Suomi",
        ["Pihtipudas"] = "Keski-Suomi", ["Saarijärvi"] = "Keski-Suomi",
        ["Toivakka"] = "Keski-Suomi", ["Uurainen"] = "Keski-Suomi",
        ["Viitasaari"] = "Keski-Suomi", ["Äänekoski"] = "Keski-Suomi",

        // Etelä-Pohjanmaa
        ["Alajärvi"] = "Etelä-Pohjanmaa", ["Alavus"] = "Etelä-Pohjanmaa",
        ["Evijärvi"] = "Etelä-Pohjanmaa", ["Ilmajoki"] = "Etelä-Pohjanmaa",
        ["Isojoki"] = "Etelä-Pohjanmaa", ["Isokyrö"] = "Etelä-Pohjanmaa",
        ["Karijoki"] = "Etelä-Pohjanmaa", ["Kauhajoki"] = "Etelä-Pohjanmaa",
        ["Kauhava"] = "Etelä-Pohjanmaa", ["Kuortane"] = "Etelä-Pohjanmaa",
        ["Kurikka"] = "Etelä-Pohjanmaa", ["Lappajärvi"] = "Etelä-Pohjanmaa",
        ["Lapua"] = "Etelä-Pohjanmaa", ["Seinäjoki"] = "Etelä-Pohjanmaa",
        ["Soini"] = "Etelä-Pohjanmaa", ["Teuva"] = "Etelä-Pohjanmaa",
        ["Vimpeli"] = "Etelä-Pohjanmaa", ["Ähtäri"] = "Etelä-Pohjanmaa",

        // Pohjanmaa
        ["Kaskinen"] = "Pohjanmaa", ["Korsnäs"] = "Pohjanmaa",
        ["Kristiinankaupunki"] = "Pohjanmaa", ["Kruunupyy"] = "Pohjanmaa",
        ["Laihia"] = "Pohjanmaa", ["Luoto"] = "Pohjanmaa",
        ["Maalahti"] = "Pohjanmaa", ["Mustasaari"] = "Pohjanmaa",
        ["Närpiö"] = "Pohjanmaa", ["Pedersören kunta"] = "Pohjanmaa",
        ["Pietarsaari"] = "Pohjanmaa", ["Uusikaarlepyy"] = "Pohjanmaa",
        ["Vaasa"] = "Pohjanmaa", ["Vöyri"] = "Pohjanmaa",

        // Keski-Pohjanmaa
        ["Halsua"] = "Keski-Pohjanmaa", ["Kannus"] = "Keski-Pohjanmaa",
        ["Kaustinen"] = "Keski-Pohjanmaa", ["Kokkola"] = "Keski-Pohjanmaa",
        ["Lestijärvi"] = "Keski-Pohjanmaa", ["Perho"] = "Keski-Pohjanmaa",
        ["Toholampi"] = "Keski-Pohjanmaa", ["Veteli"] = "Keski-Pohjanmaa",

        // Pohjois-Pohjanmaa (+8 % lämmityskorotus)
        ["Alavieska"] = "Pohjois-Pohjanmaa", ["Haapajärvi"] = "Pohjois-Pohjanmaa",
        ["Haapavesi"] = "Pohjois-Pohjanmaa", ["Hailuoto"] = "Pohjois-Pohjanmaa",
        ["Ii"] = "Pohjois-Pohjanmaa", ["Kalajoki"] = "Pohjois-Pohjanmaa",
        ["Kempele"] = "Pohjois-Pohjanmaa", ["Kuusamo"] = "Pohjois-Pohjanmaa",
        ["Kärsämäki"] = "Pohjois-Pohjanmaa", ["Liminka"] = "Pohjois-Pohjanmaa",
        ["Lumijoki"] = "Pohjois-Pohjanmaa", ["Merijärvi"] = "Pohjois-Pohjanmaa",
        ["Muhos"] = "Pohjois-Pohjanmaa", ["Nivala"] = "Pohjois-Pohjanmaa",
        ["Oulu"] = "Pohjois-Pohjanmaa", ["Oulainen"] = "Pohjois-Pohjanmaa",
        ["Pudasjärvi"] = "Pohjois-Pohjanmaa", ["Pyhäjoki"] = "Pohjois-Pohjanmaa",
        ["Pyhäjärvi"] = "Pohjois-Pohjanmaa", ["Pyhäntä"] = "Pohjois-Pohjanmaa",
        ["Raahe"] = "Pohjois-Pohjanmaa", ["Reisjärvi"] = "Pohjois-Pohjanmaa",
        ["Sievi"] = "Pohjois-Pohjanmaa", ["Siikajoki"] = "Pohjois-Pohjanmaa",
        ["Siikalatva"] = "Pohjois-Pohjanmaa", ["Taivalkoski"] = "Pohjois-Pohjanmaa",
        ["Tyrnävä"] = "Pohjois-Pohjanmaa", ["Utajärvi"] = "Pohjois-Pohjanmaa",
        ["Vaala"] = "Pohjois-Pohjanmaa", ["Ylivieska"] = "Pohjois-Pohjanmaa",

        // Kainuu (+8 % lämmityskorotus)
        ["Hyrynsalmi"] = "Kainuu", ["Kajaani"] = "Kainuu",
        ["Kuhmo"] = "Kainuu", ["Paltamo"] = "Kainuu",
        ["Puolanka"] = "Kainuu", ["Ristijärvi"] = "Kainuu",
        ["Sotkamo"] = "Kainuu", ["Suomussalmi"] = "Kainuu",

        // Lappi (+8 % lämmityskorotus)
        ["Enontekiö"] = "Lappi", ["Inari"] = "Lappi",
        ["Kemi"] = "Lappi", ["Kemijärvi"] = "Lappi",
        ["Keminmaa"] = "Lappi", ["Kittilä"] = "Lappi",
        ["Kolari"] = "Lappi", ["Muonio"] = "Lappi",
        ["Pelkosenniemi"] = "Lappi", ["Pello"] = "Lappi",
        ["Posio"] = "Lappi", ["Ranua"] = "Lappi",
        ["Rovaniemi"] = "Lappi", ["Salla"] = "Lappi",
        ["Savukoski"] = "Lappi", ["Simo"] = "Lappi",
        ["Sodankylä"] = "Lappi", ["Tervola"] = "Lappi",
        ["Tornio"] = "Lappi", ["Utsjoki"] = "Lappi",
        ["Ylitornio"] = "Lappi",

        // Ahvenanmaa
        ["Brändö"] = "Ahvenanmaa", ["Eckerö"] = "Ahvenanmaa",
        ["Finström"] = "Ahvenanmaa", ["Föglö"] = "Ahvenanmaa",
        ["Geta"] = "Ahvenanmaa", ["Hammarland"] = "Ahvenanmaa",
        ["Jomala"] = "Ahvenanmaa", ["Kumlinge"] = "Ahvenanmaa",
        ["Kökar"] = "Ahvenanmaa", ["Lemland"] = "Ahvenanmaa",
        ["Lumparland"] = "Ahvenanmaa", ["Maarianhamina"] = "Ahvenanmaa",
        ["Saltvik"] = "Ahvenanmaa", ["Sottunga"] = "Ahvenanmaa",
        ["Sund"] = "Ahvenanmaa", ["Vårdö"] = "Ahvenanmaa",
    };

    private static readonly Lazy<IReadOnlyList<string>> KaikkiKunnat = new(() =>
        KuntaToMaakunta.Keys.OrderBy(k => k, StringComparer.Create(new CultureInfo("fi"), false)).ToList()
    );

    public Kuntaryhma Hae(string kunta)
    {
        if (RyhmaI.Contains(kunta)) return Kuntaryhma.I;
        if (RyhmaII.Contains(kunta)) return Kuntaryhma.II;
        return Kuntaryhma.III;
    }

    public string? HaeMaakunta(string kunta)
        => KuntaToMaakunta.TryGetValue(kunta, out var maakunta) ? maakunta : null;

    public IReadOnlyList<string> HaeKaikkiKunnat() => KaikkiKunnat.Value;
}
