# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MarketplaceApp.Api.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/out

# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Create folder for images
RUN mkdir -p wwwroot/images

# Expose port
EXPOSE 8080
ENTRYPOINT ["dotnet", "MarketplaceApp.Api.dll"]