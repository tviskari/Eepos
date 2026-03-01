FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY kirjastot/Eepos.Kunnat/ kirjastot/Eepos.Kunnat/
COPY moduulit/asumistuki/src/Asumistuki/ moduulit/asumistuki/src/Asumistuki/
COPY moduulit/asumistuki/src/Asumistuki.Testipenkki/ moduulit/asumistuki/src/Asumistuki.Testipenkki/
RUN dotnet publish moduulit/asumistuki/src/Asumistuki.Testipenkki/Asumistuki.Testipenkki.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Asumistuki.Testipenkki.dll"]
