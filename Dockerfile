# ---------- build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar proyectos y restaurar dependencias
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
# 👆 usa SDK, no solo runtime, así sí puede correr `dotnet ef`

WORKDIR /app

# Instalar netcat para esperar a la BD
RUN apt-get update && apt-get install -y netcat-openbsd

# Copiar archivos publicados
COPY --from=build /app/publish .

# Variables de entorno
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV PATH="$PATH:/root/.dotnet/tools"

# Instalar EF CLI
RUN dotnet tool install --global dotnet-ef --version 8.0.0

EXPOSE 8080

# 🧠 Migraciones automáticas + espera de base de datos + arranque
CMD bash -c "\
    echo '⏳ Esperando base de datos...'; \
    until nc -z ${POSTGRES_HOST:-carnetizacion-postgres-staging} ${POSTGRES_PORT:-5432}; do \
        echo '🕓 Aguardando conexión con la BD...'; sleep 3; \
    done; \
    echo '🏗️ Verificando migraciones existentes...'; \
    if [ ! -d '/app/Entity/Migrations' ] || [ -z \"$(ls -A /app/Entity/Migrations 2>/dev/null)\" ]; then \
        echo '⚙️ No hay migraciones, creando una nueva...'; \
        dotnet ef migrations add AutoInitial --project Entity/Entity.csproj --startup-project Web/Web.csproj || true; \
    else \
        echo '✅ Migraciones detectadas.'; \
    fi; \
    echo '📦 Aplicando migraciones...'; \
    dotnet ef database update --project Entity/Entity.csproj --startup-project Web/Web.csproj; \
    echo '🚀 Iniciando API...'; \
    dotnet Web.dll"
