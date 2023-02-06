#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM node:latest AS node_base

RUN echo "NODE Version:" && node --version
RUN echo "NPM Version:" && npm --version

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY --from=node_base . .
WORKDIR /src
COPY ["EStore.IdentityServer/EStore.IdentityServer.csproj", "EStore.IdentityServer/"]
COPY ["EStore.Common/EStore.Common.csproj", "EStore.Common/"]
RUN dotnet restore "EStore.IdentityServer/EStore.IdentityServer.csproj"
COPY . .
WORKDIR "/src/EStore.IdentityServer"
RUN dotnet build "EStore.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EStore.IdentityServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EStore.IdentityServer.dll"]