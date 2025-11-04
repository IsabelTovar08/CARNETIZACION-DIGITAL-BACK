# ---------- build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar los archivos del proyecto y restaurar dependencias
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
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
# 👆 usamos el SDK completo también aquí (no solo el runtime) para poder usar `dotnet ef`

WORKDIR /app

# Instalar herramientas necesarias
RUN apt-get update && apt-get install -y netcat-openbsd && rm -rf /var/lib/apt/lists/*

# Copiar los artefactos publicados desde la etapa anterior
COPY --from=build /app/publish .

# Instalar EF CLI global
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="$PATH:/root/.dotnet/tools"
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 8080

# Espera a la base de datos + ejecuta migraciones + arranca la API
CMD bash -c "\
    echo '⏳ Esperando base de datos...'; \
    until nc -z ${POSTGRES_HOST:-carnetizacion-postgres-staging} ${POSTGRES_PORT:-5432}; do \
        echo '🕓 Esperando conexión con la base de datos...'; sleep 3; \
    done; \
    echo '🏗️ Verificando migraciones de Entity Framework...'; \
    if [ ! -d '/app/Entity/Migrations' ] || [ -z \"$(ls -A /app/Entity/Migrations 2>/dev/null)\" ]; then \
        echo '⚙️ No hay migraciones, creando una nueva...'; \
        dotnet ef migrations add AutoInitial --project Entity/Entity.csproj --startup-project Web/Web.csproj || true; \
    else \
        echo '✅ Migraciones existentes detectadas.'; \
    fi; \
    echo '📦 Aplicando migraciones...'; \
    dotnet ef database update --project Entity/Entity.csproj --startup-project Web/Web.csproj; \
    echo '🚀 Iniciando API...'; \
    dotnet Web.dll"
