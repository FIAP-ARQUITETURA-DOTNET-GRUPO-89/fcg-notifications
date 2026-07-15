# syntax=docker/dockerfile:1.7

########################################
# Build
########################################
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY global.json .
COPY Directory.Build.props .
COPY Directory.Packages.props .

# Projetos
COPY src/FcgNotifications.Worker/FcgNotifications.Worker.csproj src/FcgNotifications.Worker/
COPY src/FcgNotifications.Application/FcgNotifications.Application.csproj src/FcgNotifications.Application/
COPY src/FcgNotifications.Domain/FcgNotifications.Domain.csproj src/FcgNotifications.Domain/
COPY src/FcgNotifications.Infrastructure/FcgNotifications.Infrastructure.csproj src/FcgNotifications.Infrastructure/
COPY src/FcgNotifications.IoC/FcgNotifications.IoC.csproj src/FcgNotifications.IoC/
COPY src/FcgNotifications.ServiceDefaults/FcgNotifications.ServiceDefaults.csproj src/FcgNotifications.ServiceDefaults/
COPY src/FcgNotifications.SharedKernel/FcgNotifications.SharedKernel.csproj src/FcgNotifications.SharedKernel/

RUN dotnet restore src/FcgNotifications.Worker/FcgNotifications.Worker.csproj

COPY . .

RUN dotnet publish src/FcgNotifications.Worker/FcgNotifications.Worker.csproj \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

########################################
# Runtime
########################################
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

RUN apt-get update && apt-get install -y \
    libgssapi-krb5-2 \
    krb5-locales \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

USER $APP_UID

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "FcgNotifications.Worker.dll"]
