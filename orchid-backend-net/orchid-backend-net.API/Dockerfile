# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["orchid-backend-net.API/orchid-backend-net.API.csproj", "orchid-backend-net.API/"]
COPY ["orchid-backend-net.Application/orchid-backend-net.Application.csproj", "orchid-backend-net.Application/"]
COPY ["orchid-backend-net.Domain/orchid-backend-net.Domain.csproj", "orchid-backend-net.Domain/"]
COPY ["orchid-backend-net.Infrastructure/orchid-backend-net.Infrastructure.csproj", "orchid-backend-net.Infrastructure/"]
RUN dotnet restore "./orchid-backend-net.API/orchid-backend-net.API.csproj"
COPY . .
WORKDIR "/src/orchid-backend-net.API"
RUN dotnet build "./orchid-backend-net.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./orchid-backend-net.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "orchid-backend-net.API.dll"]