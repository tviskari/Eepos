# Asumistuki-moduuli — Ohje Claudelle

## Moduulin tarkoitus

Yleisen asumistuen laskuri lain 938/2014 mukaisesti. Laskee tuen määrän ruokakunnan tulojen, asumismenojen, omaisuuden ja kunnan perusteella.

## Arkkitehtuuri

Contracts/ → rajapinnat, Models/ → tietomallit (record), Services/ → toteutukset. Palvelut injektoidaan konstruktorin kautta (DI). Päälaskuri `AsumistukiLaskuri` orkestroi neljää osapalvelua.

## Laskentakaava

```
tuki = 0.70 × (min(hyväksytytMenot, enimmäisMenot) − perusomavastuuosuus)
```

## Kriittiset säännöt

- `decimal` kaikkialla eurolaskennassa — EI double/float
- Pyöristys sentteihin: `Math.Round(value, 2, MidpointRounding.AwayFromZero)` (§52)
- Tulot ja tuloraja pyöristetään täysiksi euroiksi (§52)
- Omavastuu < 10 € → nollataan (§16)
- Tuki < 15 € → ei makseta (§24)
- Omaisuus >= 50 000 € → hylkäys (§13.5)
- Vähintään 1 aikuinen (§16.2)
- Vammaisen lisätila: enimmäismenot hlömäärä + 1 mukaan (§11)

## Tiedostot

| Tiedosto | Vastuu |
|----------|--------|
| `Services/AsumistukiLaskuri.cs` | Päälaskenta, 8-vaiheinen orkestrointi |
| `Services/AsumismenotLaskenta.cs` | Vuokra + vesi + lämmitys, enimmäismenot-taulukko |
| `Services/PerusomavastuuLaskenta.cs` | Tuloraja, 50% omavastuu |
| `Services/OmaisuustuloLaskenta.cs` | Omaisuusraja, omaisuustulo |
| `Services/KuntaryhmaService.cs` | Kuntien luokittelu ryhmiin I/II/III |
| `Models/RuokakuntaInput.cs` | Syöte-record |
| `Models/AsumistukiTulos.cs` | Tulos-record |

## Testit

54 testiä, kaikki `tests/Asumistuki.Tests/`-hakemistossa. Aja: `dotnet test`.

Jokainen testi sisältää lakipykäläviitteen ja laskukaavan kommenttina. Testit käyttävät konkreettisia luokkia (ei mockeja).

## Testipenkki (Asumistuki.Testipenkki)

Blazor Server -käyttöliittymä moduulin manuaaliseen testaukseen. Sijaitsee `src/Asumistuki.Testipenkki/`.

- Lokalisointi: suomi (oletus) ja ruotsi, resx-tiedostot `Resources/`-hakemistossa
- Kielivalinta vaihtaa `CultureInfo.CurrentUICulture` suoraan Blazor-piirissä (ei cookieta)
- DI-rekisteröinti `Program.cs`:ssä — kaikki laskentapalvelut Scoped-elinkaarisin
- Käynnistys: `dotnet run --project src/Asumistuki.Testipenkki` → `http://localhost:5233`

## Muutoksia tehdessä

- Lakipykälät on kommentoitu koodiin — tarkista vastaava pykälä ennen muutosta
- Euromäärät ovat lakitekstin perusarvoja, indeksitarkistus tuotannossa
- Lisää aina testi uudelle toiminnallisuudelle, viittaa pykälään
- Enimmäismenot-taulukko on `AsumismenotLaskenta.cs`:n `Enimmaismenot`-dictionaryssa
- Kuntalistat ovat `KuntaryhmaService.cs`:n HashSet-rakenteissa
