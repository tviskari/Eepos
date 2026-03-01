# Eepos — Moduulit

Eepos-järjestelmän laskentamoduulit. Jokainen moduuli on itsenäinen .NET-ratkaisu omassa hakemistossaan.

## Moduulit

| Moduuli | Hakemisto | Kuvaus | Laki |
|---------|-----------|--------|------|
| **Asumistuki** | `asumistuki/` | Yleisen asumistuen laskuri | 938/2014 (Laki yleisestä asumistuesta) |

## Rakenne

Jokainen moduuli noudattaa yhtenäistä rakennetta:

```
moduuli/
├── src/
│   └── Moduuli/
│       ├── Contracts/    # Rajapinnat
│       ├── Models/       # Tietomallit
│       └── Services/     # Toteutukset
├── tests/
│   └── Moduuli.Tests/
├── README.md
└── CLAUDE.md
```

## Testien ajaminen

Yksittäinen moduuli:
```bash
cd asumistuki && dotnet test
```

Kaikki moduulit:
```bash
for dir in */; do (cd "$dir" && dotnet test); done
```
