# Infra

Eepos-järjestelmän infrastruktuurikonfiguraatiot. Jokainen pilvialusta on omassa hakemistossaan.

## Rakenne

```
infra/
└── azure/     # Azure Container Apps
```

## Edellytykset

- [Terraform](https://www.terraform.io/) >= 1.5
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/) (kirjautuneena: `az login`)
- [Docker](https://www.docker.com/)

## Kontit

| Kontti | Dockerfile | Kuvaus |
|--------|-----------|--------|
| `asumistuki-api` | `moduulit/asumistuki/src/Asumistuki.Api/Dockerfile` | REST API + Swagger |
| `asumistuki-testipenkki` | `moduulit/asumistuki/src/Asumistuki.Testipenkki/Dockerfile` | Blazor Server -testikäyttöliittymä |

## Azure Container Apps

### 1. Docker-imagejen rakennus

```bash
# Repon juuresta (Eepos/)
docker build -t asumistuki-api -f moduulit/asumistuki/src/Asumistuki.Api/Dockerfile .
docker build -t asumistuki-testipenkki -f moduulit/asumistuki/src/Asumistuki.Testipenkki/Dockerfile .
```

### 2. Terraform

```bash
cd infra/azure
cp terraform.tfvars.example terraform.tfvars
# Muokkaa terraform.tfvars tarvittaessa

terraform init
terraform plan
terraform apply
```

### 3. Imagejen push ACR:iin

```bash
# Kirjaudu ACR:iin (terraform output antaa osoitteen)
az acr login --name <acr-nimi>

# API
docker tag asumistuki-api <acr-osoite>/asumistuki-api:latest
docker push <acr-osoite>/asumistuki-api:latest

# Testipenkki
docker tag asumistuki-testipenkki <acr-osoite>/asumistuki-testipenkki:latest
docker push <acr-osoite>/asumistuki-testipenkki:latest
```

### 4. Paikallinen testaus

```bash
# API
docker run -p 8080:8080 asumistuki-api
# Swagger: http://localhost:8080/swagger
# API: POST http://localhost:8080/api/asumistuki/laske

# Testipenkki
docker run -p 8081:8080 asumistuki-testipenkki
# UI: http://localhost:8081
```

## Uuden pilvialustan lisääminen

1. Luo hakemisto `infra/<alusta>/` (esim. `infra/aws/`)
2. Lisää Terraform-konfiguraatio samalla muuttujarakenteella
3. Dockerfilet ovat yhteisiä — vain infra vaihtuu
