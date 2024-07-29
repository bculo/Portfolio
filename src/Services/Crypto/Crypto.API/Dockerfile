FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Crypto/Crypto.API/Crypto.API.csproj", "src/Services/Crypto/Crypto.API/"]
COPY ["src/BuildingBlocks/Utility/WebProject.Common/WebProject.Common.csproj", "src/BuildingBlocks/Utility/WebProject.Common/"]
COPY ["src/BuildingBlocks/Utility/Cryptography.Common/Cryptography.Common.csproj", "src/BuildingBlocks/Utility/Cryptography.Common/"]
COPY ["src/Services/Crypto/Crypto.Infrastructure/Crypto.Infrastructure.csproj", "src/Services/Crypto/Crypto.Infrastructure/"]
COPY ["src/BuildingBlocks/Utility/Queryable.Common/Queryable.Common.csproj", "src/BuildingBlocks/Utility/Queryable.Common/"]
COPY ["src/Services/Crypto/Crypto.Application/Crypto.Application.csproj", "src/Services/Crypto/Crypto.Application/"]
COPY ["src/BuildingBlocks/Events/Events.Common/Events.Common.csproj", "src/BuildingBlocks/Events/Events.Common/"]
COPY ["src/BuildingBlocks/Utility/Keycloak.Common/Keycloak.Common.csproj", "src/BuildingBlocks/Utility/Keycloak.Common/"]
COPY ["src/BuildingBlocks/Abstract/Auth0.Abstract/Auth0.Abstract.csproj", "src/BuildingBlocks/Abstract/Auth0.Abstract/"]
COPY ["src/BuildingBlocks/Utility/Time.Common/Time.Common.csproj", "src/BuildingBlocks/Utility/Time.Common/"]
COPY ["src/BuildingBlocks/Abstract/Time.Abstract/Time.Abstract.csproj", "src/BuildingBlocks/Abstract/Time.Abstract/"]
COPY ["src/BuildingBlocks/Utility/Utility.Common/Http.Common.csproj", "src/BuildingBlocks/Utility/Utility.Common/"]
COPY ["src/BuildingBlocks/Utility/Cache.Redis.Common/Cache.Redis.Common.csproj", "src/BuildingBlocks/Utility/Cache.Redis.Common/"]
COPY ["src/Services/Crypto/Crypto.Core/Crypto.Core.csproj", "src/Services/Crypto/Crypto.Core/"]
RUN dotnet restore "src/Services/Crypto/Crypto.API/Crypto.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Crypto/Crypto.API"
RUN dotnet build "Crypto.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Crypto.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crypto.API.dll"]
