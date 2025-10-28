# ===================================================================
# <summary>
#  Etapa de compilación (Build Stage)
#  - Instala .NET SDK 8.0
#  - Restaura dependencias
#  - Aplica migraciones de Entity Framework
#  - Publica la API en modo Release
# </summary>
# ===================================================================
FROM ubuntu:22.04 AS build

# <summary>
# Instalar dependencias del sistema y SDK de .NET 8.0
# </summary>
RUN apt-get update && apt-get install -y wget apt-transport-https software-properties-common \
    && wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && apt-get update && apt-get install -y dotnet-sdk-8.0

WORKDIR /src

# <summary>
# Copiar archivos de proyecto (.csproj) y restaurar dependencias NuGet
# </summary>
COPY *.sln ./
COPY Web/*.csproj Web/
COPY Business/*.csproj Business/
COPY Data/*.csproj Data/
COPY Entity/*.csproj Entity/
COPY Utilities/*.csproj Utilities/
COPY Diagram/*.csproj Diagram/

RUN dotnet restore Web/Web.csproj

# <summary>
# Copiar todo el código fuente
# </summary>
COPY . .

# <summary>
# Instalar la herramienta EF Core para ejecutar migraciones
# </summary>
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="$PATH:/root/.dotnet/tools"

# <summary>
# Ejecutar las migraciones de Entity Framework durante el build
# </summary>
RUN dotnet ef database update --project Entity/Entity.csproj --startup-project Web/Web.csproj

# <summary>
# Publicar la aplicación compilada en modo Release
# </summary>
RUN dotnet publish Web/Web.csproj -c Release -o /app/publish

# ===================================================================
# <summary>
#  Etapa final (Runtime Stage)
#  - Usa el runtime de .NET 8.0
#  - Copia la publicación ya migrada
#  - Expone el puerto y arranca la API
# </summary>
# ===================================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Web.dll"]
