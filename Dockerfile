# ---------- build & runtime stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final

WORKDIR /app

# Copiar y restaurar
COPY *.sln ./
COPY Web/*.csproj Web/
COPY Business/*.csproj Business/
COPY Data/*.csproj Data/
COPY Entity/*.csproj Entity/
COPY Utilities/*.csproj Utilities/
COPY Diagram/*.csproj Diagram/

RUN dotnet restore Web/Web.csproj

# Copiar todo y publicar
COPY . .
RUN dotnet publish Web/Web.csproj -c Release -o /app/publish

# Instalar EF CLI
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="${PATH}:/root/.dotnet/tools"

WORKDIR /app/publish
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080 DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "Web.dll"]
