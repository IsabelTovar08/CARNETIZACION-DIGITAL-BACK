# ---------- build stage ----------
FROM ubuntu:22.04 AS build

# Instalar dependencias básicas
RUN apt-get update && apt-get install -y curl git ca-certificates

# Instalar .NET SDK 8 manualmente (funciona en ARM y x64)
RUN curl -L https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh && \
    chmod +x dotnet-install.sh && \
    ./dotnet-install.sh --channel 8.0 --install-dir /usr/share/dotnet && \
    ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

WORKDIR /src

# Copiar archivos de proyecto
COPY *.sln ./
COPY Web/*.csproj Web/
COPY Business/*.csproj Business/
COPY Data/*.csproj Data/
COPY Entity/*.csproj Entity/
COPY Utilities/*.csproj Utilities/
COPY Diagram/*.csproj Diagram/

RUN dotnet --info
RUN dotnet restore Web/Web.csproj
COPY . .
RUN dotnet publish Web/Web.csproj -c Release -o /app/publish


# ---------- runtime stage ----------
FROM ubuntu:22.04 AS final

# Instalar runtime y SDK para migraciones
RUN apt-get update && apt-get install -y curl git netcat-openbsd ca-certificates && \
    curl -L https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh && \
    chmod +x dotnet-install.sh && \
    ./dotnet-install.sh --channel 8.0 --install-dir /usr/share/dotnet && \
    ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet && \
    dotnet tool install --global dotnet-ef --version 8.0.0

ENV PATH="$PATH:/root/.dotnet/tools"
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true

WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080

# Esperar BD + ejecutar migraciones + arrancar API
CMD bash -c "\
    echo '⏳ Esperando base de datos...'; \
    until nc -z ${POSTGRES_HOST:-carnetizacion-postgres-staging} ${POSTGRES_PORT:-5432}; do \
        echo '🕓 Esperando conexión con la BD...'; sleep 3; \
    done; \
    echo '🏗️ Verificando migraciones EF...'; \
    if [ ! -d '/app/Entity/Migrations' ] || [ -z \"$(ls -A /app/Entity/Migrations 2>/dev/null)\" ]; then \
        echo '⚙️ Creando migración inicial...'; \
        dotnet ef migrations add AutoInitial --project Entity/Entity.csproj --startup-project Web/Web.csproj || true; \
    fi; \
    echo '📦 Aplicando migraciones...'; \
    dotnet ef database update --project Entity/Entity.csproj --startup-project Web/Web.csproj; \
    echo '🚀 Iniciando API...'; \
    dotnet Web.dll"
