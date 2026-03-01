# Asumistuki-laskuri

Yleisen asumistuen laskuri perustuen lakiin 938/2014 (Laki yleisestä asumistuesta).

## Yleiskuvaus

Laskuri laskee yleisen asumistuen määrän ruokakunnan tietojen perusteella. Laskenta perustuu kaavaan:

```
tuki = 0.70 × (min(hyväksytytAsumismenot, enimmäisasumismenot) − perusomavastuuosuus)
```

Keskeisiä sääntöjä:
- Tukea ei makseta, jos sen määrä on alle 15 €/kk (§24)
- Perusomavastuuta ei huomioida, jos se on alle 10 € (§16)
- Omaisuus >= 50 000 € estää tuen myöntämisen (§13.5)

## Tekniset vaatimukset

- .NET 8+
- C# 12
- xUnit (testit)

## Projektirakenne

```
asumistuki/
├── src/
│   ├── Asumistuki/                     # Laskentakirjasto
│   │   ├── Contracts/                  # Rajapinnat
│   │   │   ├── IAsumistukiLaskuri.cs
│   │   │   ├── IAsumismenotLaskenta.cs
│   │   │   ├── IPerusomavastuuLaskenta.cs
│   │   │   ├── IOmaisuustuloLaskenta.cs
│   │   │   └── IKuntaryhmaService.cs
│   │   ├── Models/                     # Tietomallit
│   │   │   ├── RuokakuntaInput.cs
│   │   │   ├── AsumistukiTulos.cs
│   │   │   └── Kuntaryhma.cs
│   │   └── Services/                   # Toteutukset
│   │       ├── AsumistukiLaskuri.cs
│   │       ├── AsumismenotLaskenta.cs
│   │       ├── PerusomavastuuLaskenta.cs
│   │       ├── OmaisuustuloLaskenta.cs
│   │       └── KuntaryhmaService.cs
│   └── Asumistuki.Testipenkki/         # Blazor Server -testikäyttöliittymä
│       ├── Components/Pages/Laskuri.razor
│       └── Resources/Lokalisointi.resx (fi), .sv.resx (sv)
└── tests/Asumistuki.Tests/
    ├── AsumistukiLaskuriTests.cs
    ├── AsumismenotLaskentaTests.cs
    ├── PerusomavastuuLaskentaTests.cs
    ├── OmaisuustuloLaskentaTests.cs
    └── KuntaryhmaServiceTests.cs
```

## Laskentavaiheet

`AsumistukiLaskuri.Laske()` suorittaa vaiheet järjestyksessä:

1. **Omaisuustarkistus** (§13.5) — hylkäys jos >= 50 000 €
2. **Omaisuustulo** (§13) — lisätään bruttotuloihin
3. **Hyväksyttävät asumismenot** (§9) — vuokra + vesi + lämmitys - alivuokralainen
4. **Enimmäisasumismenot** (§10, §11) — kuntaryhmän ja henkilömäärän mukaan
5. **Huomioitavat menot** — min(hyväksytyt, enimmäis)
6. **Perusomavastuuosuus** (§16) — 50 % × (tulot − tuloraja)
7. **Tuen laskenta** — 0.70 × max(0, huomioitavat − omavastuu)
8. **Maksetaanko** (§24) — tuki >= 15 €

## Kuntaryhmät (§10.3)

| Ryhmä | Kunnat |
|-------|--------|
| I | Helsinki, Espoo, Kauniainen, Vantaa |
| II | Hyvinkää, Hämeenlinna, Joensuu, Jyväskylä, Järvenpää, Kerava, Kirkkonummi, Kuopio, Lahti, Lohja, Nokia, Nurmijärvi, Oulu, Porvoo, Raisio, Riihimäki, Rovaniemi, Seinäjoki, Sipoo, Siuntio, Tampere, Turku, Tuusula, Vihti |
| III | Kaikki muut |

## Käyttöesimerkki

```csharp
var laskuri = new AsumistukiLaskuri(
    new AsumismenotLaskenta(),
    new PerusomavastuuLaskenta(),
    new OmaisuustuloLaskenta(),
    new KuntaryhmaService()
);

var input = new RuokakuntaInput
{
    Aikuiset = 1,
    Lapset = 0,
    Kunta = "Helsinki",
    Vuokra = 800m,
    BruttotulotKk = 0m
};

var tulos = laskuri.Laske(input);
// tulos.TuenMaara = 344.40m
// tulos.Maksetaanko = true
```

## Testipenkki (Asumistuki.Testipenkki)

Blazor Server -käyttöliittymä laskurimoduulin manuaaliseen testaukseen. Kieli vaihdettavissa suomen ja ruotsin välillä.

### Käynnistys

```bash
dotnet run --project src/Asumistuki.Testipenkki
```

Avautuu osoitteessa `http://localhost:5233`.

## Testien ajaminen

```bash
dotnet test
```

## Huomautuksia

- **Indeksitarkistukset** (§51): Euromäärät ovat lakitekstin perusarvoja. Tuotannossa indeksitarkistettava.
- **Etuoikeutetut tulot** (§15): Oletetaan syötteen olevan jo puhdistettu.
- **Omistusasunnot**: Eivät enää oikeutettuja 1.1.2025 alkaen.
- Tulokset voi validoida [Kelan laskurilla](https://www.kela.fi/laskurit).
