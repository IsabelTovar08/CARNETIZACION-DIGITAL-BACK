# ---------- build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
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
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS final
WORKDIR /app
COPY --from=build /app/publish .

# Instalar EF CLI
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="$PATH:/root/.dotnet/tools"
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true
EXPOSE 8080

# ---------- arranque automático ----------
CMD bash -c "\
    echo '⏳ Esperando base de datos...'; \
    until nc -z ${POSTGRES_HOST:-carnetizacion-postgres-staging} ${POSTGRES_PORT:-5432}; do \
        echo '🕓 Esperando conexión con la BD...'; sleep 3; \
    done; \
    echo '🏗️ Revisando migraciones...'; \
    if [ ! -d '/app/Entity/Migrations' ] || [ -z \"$(ls -A /app/Entity/Migrations 2>/dev/null)\" ]; then \
        echo '⚙️ No hay migraciones, creando AutoInitial...'; \
        dotnet ef migrations add AutoInitial --project Entity/Entity.csproj --startup-project Web/Web.csproj || true; \
    fi; \
    echo '📦 Aplicando migraciones...'; \
    dotnet ef database update --project Entity/Entity.csproj --startup-project Web/Web.csproj; \
    echo '🚀 Iniciando API...'; \
    dotnet Web.dll"
