# Eepos — Ohje Claudelle

## Projektin tarkoitus

Eepos on suomalaisen sosiaaliturvalainsäädännön laskentajärjestelmä. Moduulit toteuttavat etuuslaskentaa lakien perusteella.

## Hakemistorakenne

```
Eepos/
├── kirjastot/
│   └── Eepos.Kunnat/     # Yhteinen kuntakirjasto (kuntaryhmät, maakunnat, 307 kuntaa)
└── moduulit/
    └── asumistuki/        # Yleinen asumistuki (laki 938/2014)
```

Moduulikohtaiset CLAUDE.md-tiedostot sisältävät yksityiskohtaiset ohjeet.

## Yleiset koodauskäytänteet

### Kieli ja nimeäminen

- **Kieli**: C# 12, .NET 8+
- **Luokat ja rajapinnat**: PascalCase (`AsumistukiLaskuri`, `IPerusomavastuuLaskenta`)
- **Rajapinnat**: I-prefiksi (`IAsumistukiLaskuri`)
- **Metodit**: PascalCase (`Laske`, `Hae`, `LaskeHyvaksytytMenot`)
- **Paikalliset muuttujat ja parametrit**: camelCase (`tulot`, `aikuiset`, `henkilomaara`)
- **Privaattikentät**: alaviiva-prefiksi (`_asumismenot`, `_kuntaryhma`)
- **Staattiset kentät**: PascalCase (`Enimmaismenot`, `RyhmaI`)
- **Domaininimet suomeksi**: luokat, metodit, muuttujat ja testit nimetään suomeksi koska domain on suomalainen lainsäädäntö
- **Tiedostonimet**: sama kuin luokan/rajapinnan nimi (`AsumistukiLaskuri.cs`)

### Arkkitehtuuri

- **Rajapintapohjainen**: jokaiselle palvelulle rajapinta `Contracts/`-hakemistossa
- **DI-konstruktori**: palvelut saavat riippuvuutensa konstruktorin kautta
- **Record-mallit**: tietomallit ovat `record`-tyyppisiä `{ get; init; }` -propertyillä
- **Enumit**: kategoriatyyppiset arvot enum-rakenteina
- **Yksi luokka per tiedosto**

### Moduulirakenne

Jokainen moduuli noudattaa samaa rakennetta:
```
moduuli/
├── src/Moduuli/
│   ├── Contracts/     # Rajapinnat
│   ├── Models/        # Tietomallit (record, enum)
│   └── Services/      # Toteutukset
├── tests/Moduuli.Tests/
├── README.md
└── CLAUDE.md
```

### Eurolaskenta

- **Aina `decimal`** — ei koskaan `double` tai `float`
- **Pyöristys**: `Math.Round(value, 2, MidpointRounding.AwayFromZero)` sentteihin
- **Täydet eurot**: `Math.Round(value, 0, MidpointRounding.AwayFromZero)` kun laki vaatii

### Testaus

- **Kehys**: xUnit
- **TDD**: testit ensin, toteutus sitten
- **Konkreettiset luokat**: testit käyttävät oikeita toteutuksia, ei mockeja
- **Lakiviitteet**: jokainen testi sisältää lakipykäläviitteen kommenttina
- **Laskukaava**: testikommentti näyttää laskutoimituksen auki
- **Nimeäminen**: kuvaava suomenkielinen nimi (`TulotAlleTulorajan_OmavastuuNolla`)
- **Theory + InlineData**: parametrisoidut testit taulukkoarvoille
- **Testien ajaminen**: `dotnet test` moduulin hakemistossa

### Hakutaulut ja vakiot

- **Dictionary**: hakutauluille kuten enimmäismenot `Dictionary<(Kuntaryhma, int), decimal>`
- **HashSet**: luokittelulle kuten kuntaryhmät `HashSet<string>(StringComparer.OrdinalIgnoreCase)`
- **Case-insensitive**: kaikki merkkijonovertailut `StringComparer.OrdinalIgnoreCase`

### Lakisäädösten käsittely

- Kommentoi lakipykälä koodiin: `// §16: tuloraja = 555 + 78 × aikuiset + 246 × lapset`
- XML-dokumentaatiokommentit rajapintoihin: `/// <summary>§13: Tulo omaisuudesta.</summary>`
- Euromäärät ovat lakitekstin perusarvoja — tuotannossa indeksitarkistettava (§51)
- Jokainen laskentasääntö on jäljitettävissä tiettyyn pykälään

### Mitä välttää

- `double` tai `float` rahamäärissä
- Mock-kirjastoja testeissä (käytä oikeita toteutuksia)
- Englanninkielisiä domain-nimiä (suomalainen lainsäädäntö → suomeksi)
- Monimutkaisia abstraktioita — pidä laskentalogiikka suoraviivaisena
- Turhia kommentteja itsestäänselvälle koodille

### Muutosten tekeminen

1. Lue moduulin CLAUDE.md ennen muutoksia
2. Tarkista vastaava lakipykälä
3. Kirjoita testi ensin (lakiviite + laskukaava kommenttiin)
4. Toteuta muutos
5. Aja `dotnet test` — kaikkien testien pitää mennä läpi
6. Päivitä README.md jos rajapinta tai rakenne muuttuu
