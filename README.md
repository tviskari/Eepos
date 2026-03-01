# Eepos

Suomalaisen sosiaaliturvalainsäädännön laskentajärjestelmä. Jokainen etuus on toteutettu omana moduulinaan.

## Rakenne

```
Eepos/
├── kirjastot/             # Yhteiset kirjastot
│   └── Eepos.Kunnat/     # Kuntadata: kuntaryhmät, maakunnat (307 kuntaa)
├── moduulit/              # Laskentamoduulit
│   └── asumistuki/        # Yleinen asumistuki (938/2014)
└── infra/                 # Infrastruktuuri (Terraform)
    └── azure/             # Azure Container Apps
```

## Moduulit

| Moduuli | Kuvaus | Laki |
|---------|--------|------|
| [Asumistuki](moduulit/asumistuki/) | Yleisen asumistuen laskuri | 938/2014 |

## Tekniset vaatimukset

- .NET 8+
- C# 12
- xUnit (testit)

## Testien ajaminen

```bash
cd moduulit/asumistuki && dotnet test
```

## Periaatteet

- **Lakiperustaisuus** — jokainen laskentasääntö on jäljitettävissä lakipykälään
- **TDD** — testit kirjoitetaan ennen toteutusta, lakipykäläviitteet kommenteissa
- **Tarkkuus** — `decimal`-tyyppi kaikessa eurolaskennassa
- **Rajapintapohjainen** — palvelut rajapintojen takana, DI-konstruktori
