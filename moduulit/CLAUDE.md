# Moduulit — Ohje Claudelle

## Yleiskuvaus

`moduulit/`-hakemisto sisältää Eepos-järjestelmän itsenäiset laskentamoduulit. Jokainen moduuli on oma .NET-solutionsa.

## Moduulit

| Moduuli | Hakemisto | Tarkoitus |
|---------|-----------|-----------|
| Asumistuki | `asumistuki/` | Yleisen asumistuen laskuri (laki 938/2014) |

## Moduulin rakenne

Kaikki moduulit noudattavat samaa rakennetta:
- `Contracts/` — rajapinnat (I-prefiksillä)
- `Models/` — record-tyyppiset tietomallit ja enumit
- `Services/` — palvelutoteutukset, DI-konstruktori

## Uuden moduulin lisääminen

1. Luo hakemisto `moduulit/uusimoduuli/`
2. `dotnet new sln`, `dotnet new classlib -o src/Moduuli`, `dotnet new xunit -o tests/Moduuli.Tests`
3. Noudata Contracts/Models/Services-rakennetta
4. Lisää README.md ja CLAUDE.md moduuliin
5. Päivitä tämä tiedosto ja `moduulit/README.md`

## Testaus

Aja moduulin testit: `cd moduuli && dotnet test`
