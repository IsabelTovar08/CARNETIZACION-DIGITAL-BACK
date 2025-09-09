
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia solución y TODOS los .csproj referenciados por el Web
COPY *.sln ./
COPY Web/*.csproj Web/
COPY Business/*.csproj Business/
COPY Data/*.csproj Data/
COPY Entity/*.csproj Entity/
COPY Utilities/*.csproj Utilities/
COPY TemplateEngineHost/*.csproj TemplateEngineHost/
COPY Diagram/*.csproj Diagram/

# Restaura SOLO el proyecto Web (ajusta ruta si tu API no está aquí)
RUN dotnet restore Web/Web.csproj

# Copia todo el código y publica el proyecto Web
COPY . .
RUN dotnet publish Web/Web.csproj -c Release -o /app/publish

######## RUNTIME ########
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080 DOTNET_RUNNING_IN_CONTAINER=true
COPY --from=build /app/publish .
EXPOSE 8080

ENTRYPOINT ["dotnet", "Web.dll"]
