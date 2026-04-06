# Base runtime
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
WORKDIR /app

# Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Localiza.MerchantGuide/Localiza.MerchantGuide.csproj", "src/Localiza.MerchantGuide/"]

RUN dotnet restore "src/Localiza.MerchantGuide/Localiza.MerchantGuide.csproj"

# Copia o resto
COPY . .

WORKDIR "/src/src/Localiza.MerchantGuide"
RUN dotnet build "Localiza.MerchantGuide.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Localiza.MerchantGuide.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Localiza.MerchantGuide.dll"]