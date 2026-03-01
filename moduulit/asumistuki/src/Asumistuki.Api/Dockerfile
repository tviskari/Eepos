FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY kirjastot/Eepos.Kunnat/ kirjastot/Eepos.Kunnat/
COPY moduulit/asumistuki/src/Asumistuki/ moduulit/asumistuki/src/Asumistuki/
COPY moduulit/asumistuki/src/Asumistuki.Api/ moduulit/asumistuki/src/Asumistuki.Api/
RUN dotnet publish moduulit/asumistuki/src/Asumistuki.Api/Asumistuki.Api.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Asumistuki.Api.dll"]
