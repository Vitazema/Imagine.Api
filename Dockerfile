﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
EXPOSE 5000
WORKDIR /app

RUN apt-get update \
    && apt-get install -y curl jq
    
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY Imagine.sln ./
COPY ["src/Imagine.Api/Imagine.Api.csproj", "./src/Imagine.Api/"]
COPY ["src/Imagine.Infrastructure.Persistence/Imagine.Infrastructure.Persistence.csproj", "./src/Imagine.Infrastructure.Persistence/"]
COPY ["src/Imagine.Core/Imagine.Core.csproj", "./src/Imagine.Core/"]
COPY ["src/Imagine.Auth/Imagine.Auth.csproj", "./src/Imagine.Auth/"]
RUN dotnet restore
COPY . .
WORKDIR "/source/src/Imagine.Api"
RUN dotnet build -c Release -o /app/build "Imagine.Api.csproj" 

FROM build AS publish
RUN dotnet publish "Imagine.Api.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Imagine.Api.dll"]
