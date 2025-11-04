# ---------- build stage ----------
FROM ubuntu:22.04 AS build

# Instalar dependencias básicas y .NET SDK
RUN apt-get update && apt-get install -y wget apt-transport-https software-properties-common \
    && wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && apt-get update && apt-get install -y dotnet-sdk-8.0

WORKDIR /src

# Copiar proyectos
COPY *.sln ./
COPY Web/*.csproj Web/
COPY Business/*.csproj Business/
COPY Data/*.csproj Data/
COPY Entity/*.csproj Entity/
COPY Utilities/*.csproj Utilities/
COPY Diagram/*.csproj Diagram/

RUN dotnet restore Web/Web.csproj
COPY . .
RUN dotnet publish Web/Web.csproj -c Release -o /app/publish

# ---------- runtime stage ----------
FROM ubuntu:22.04 AS final

# ⚙️ Instalar runtime y herramientas EF
RUN apt-get update && apt-get install -y wget apt-transport-https software-properties-common netcat-openbsd \
    && wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && apt-get update && apt-get install -y aspnetcore-runtime-8.0 dotnet-sdk-8.0 \
    && dotnet tool install --global dotnet-ef --version 8.0.0 \
    && rm -rf /var/lib/apt/lists/*

ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080 DOTNET_RUNNING_IN_CONTAINER=true
COPY --from=build /app/publish .
EXPOSE 8080

# 🧠 Crea migración si no existe y luego aplica
CMD bash -c "\
    echo '🏗️ Verificando migraciones de Entity Framework...'; \
    if [ ! -d '/app/Migrations' ] || [ -z \"$(ls -A /app/Migrations 2>/dev/null)\" ]; then \
        echo '⚙️ No hay migraciones existentes, creando una nueva...'; \
        dotnet ef migrations add AutoInitial --project Entity/Entity.csproj --startup-project Web/Web.csproj || echo '⚠️ No se pudo crear la migración (puede que ya exista)'; \
    else \
        echo '✅ Migraciones detectadas.'; \
    fi; \
    echo '📦 Aplicando migraciones a la base de datos...'; \
    dotnet ef database update --project Entity/Entity.csproj --startup-project Web/Web.csproj; \
    echo '🚀 Iniciando aplicación...'; \
    dotnet Web.dll"
