# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Install Node.js
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs

COPY ["src/PermissionsApp.Server/PermissionsApp.API.csproj", "src/PermissionsApp.Server/"]
COPY ["src/PermissionsApp.Infraestructure/PermissionsApp.Infraestructure.csproj", "src/PermissionsApp.Infraestructure/"]
COPY ["src/PermissionsApp.Application/PermissionsApp.Application.csproj", "src/PermissionsApp.Application/"]
COPY ["src/PermissionsApp.Domain/PermissionsApp.Domain.csproj", "src/PermissionsApp.Domain/"]
RUN dotnet restore "./src/PermissionsApp.Server/PermissionsApp.API.csproj"
COPY . .
WORKDIR "/src/src/PermissionsApp.Server"
RUN dotnet build "./PermissionsApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PermissionsApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PermissionsApp.API.dll"]