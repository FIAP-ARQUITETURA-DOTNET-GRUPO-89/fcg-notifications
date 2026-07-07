# ==========================================
# Build
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Arquivos compartilhados da solução
COPY global.json .
COPY Directory.Build.props .
COPY Directory.Packages.props .

# Copia apenas os csproj para aproveitar o cache do Docker
COPY src/FcgNotifications.Api/FcgNotifications.Api.csproj src/FcgNotifications.Api/
COPY src/FcgNotifications.Application/FcgNotifications.Application.csproj src/FcgNotifications.Application/
COPY src/FcgNotifications.Domain/FcgNotifications.Domain.csproj src/FcgNotifications.Domain/
COPY src/FcgNotifications.Infrastructure/FcgNotifications.Infrastructure.csproj src/FcgNotifications.Infrastructure/
COPY src/FcgNotifications.IoC/FcgNotifications.IoC.csproj src/FcgNotifications.IoC/
COPY src/FcgNotifications.SharedKernel/FcgNotifications.SharedKernel.csproj src/FcgNotifications.SharedKernel/

RUN dotnet restore src/FcgNotifications.Api/FcgNotifications.Api.csproj

# Copia todo o restante do código
COPY . .

WORKDIR /src/src/FcgNotifications.Api

RUN dotnet publish \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# ==========================================
# Runtime
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0

USER $APP_UID

WORKDIR /app

EXPOSE 7073

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "FcgNotifications.Api.dll"]