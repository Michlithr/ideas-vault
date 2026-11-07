FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY IdeasVault.Api/IdeasVault.Api.csproj ./IdeasVault.Api/
RUN dotnet restore IdeasVault.Api/IdeasVault.Api.csproj

COPY IdeasVault.Api/ ./IdeasVault.Api/
RUN dotnet publish IdeasVault.Api/IdeasVault.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT [ "dotnet", "IdeasVault.Api.dll" ]






