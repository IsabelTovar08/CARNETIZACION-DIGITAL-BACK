# ===================================================================
# <summary>
#  Etapa de compilación (Build Stage)
#  - Basada en Ubuntu 22.04
#  - Instala el SDK de .NET 8.0 para compilar la solución
#  - Restaura dependencias y publica el proyecto en modo Release
# </summary>
# ===================================================================
FROM ubuntu:22.04 AS build

# <summary>
# Instalar dependencias del sistema y el SDK de .NET 8.0
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
# Copiar el código fuente y publicar en modo Release
# </summary>
COPY . .
RUN dotnet publish Web/Web.csproj -c Release -o /app/publish

# ===================================================================
# <summary>
#  Etapa final (Runtime Stage)
#  - Basada en Ubuntu 22.04 con SDK completo
#  - Permite ejecutar migraciones EF Core dentro del contenedor
#  - Inicia automáticamente la API
# </summary>
# ===================================================================
FROM ubuntu:22.04 AS final

# <summary>
# Instalar dependencias del sistema y el SDK de .NET 8.0
# (Se usa el SDK completo, no solo el runtime, para soportar 'dotnet ef')
# </summary>
RUN apt-get update && apt-get install -y wget apt-transport-https software-properties-common \
    && wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && apt-get update && apt-get install -y dotnet-sdk-8.0

# <summary>
# Instalar la herramienta global de Entity Framework Core
# Permite ejecutar 'dotnet ef database update' dentro del contenedor.
# </summary>
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true

# <summary>
# Copiar la publicación generada desde la etapa de build
# </summary>
COPY --from=build /app/publish .

# <summary>
# Exponer el puerto de la API
# </summary>
EXPOSE 8080

# <summary>
# Comando de inicio por defecto
# Si el docker-compose lo sobrescribe con migraciones, este será ignorado.
# </summary>
ENTRYPOINT ["dotnet", "Web.dll"]
