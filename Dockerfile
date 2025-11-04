# ---------- build stage ----------
FROM ubuntu:22.04 AS build

# Instalar dependencias básicas y .NET SDK
RUN apt-get update && apt-get install -y wget apt-transport-https software-properties-common netcat-openbsd \
    && wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && apt-get update && apt-get install -y dotnet-sdk-8.0

# Instalar dotnet-ef en el build stage
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /src

# Copia de proyectos
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

# ---------- migration stage ----------
FROM ubuntu:22.04 AS migration

RUN apt-get update && apt-get install -y wget apt-transport-https software-properties-common netcat-openbsd \
    && wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && apt-get update && apt-get install -y dotnet-sdk-8.0

# Instalar dotnet-ef
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /src

# Copiar código fuente completo (necesario para migraciones)
COPY --from=build /src .

# Este stage se usará para ejecutar migraciones

# ---------- runtime stage ----------
FROM ubuntu:22.04 AS final

RUN apt-get update && apt-get install -y wget apt-transport-https software-properties-common \
    && wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && apt-get update && apt-get install -y aspnetcore-runtime-8.0

WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080 DOTNET_RUNNING_IN_CONTAINER=true
COPY --from=build /app/publish .
EXPOSE 8080

ENTRYPOINT ["dotnet", "Web.dll"]