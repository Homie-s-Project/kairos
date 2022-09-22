# Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
# Variable d'environement
ENV ASPNETCORE_URLS=http://+:5001
ENV ASPNETCORE_ENVIRONMENT="development"
COPY . .
RUN dotnet restore "./Kairos.API/Kairos.API.csproj" --disable-parallel
RUN dotnet publish "./Kairos.API/Kairos.API.csproj" -c release -o /app --no-restore

# Serve
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5001

ENTRYPOINT ["dotnet", "Kairos.API.dll"]