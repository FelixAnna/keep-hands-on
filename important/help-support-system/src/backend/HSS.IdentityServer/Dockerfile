#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM node:latest AS node_base

RUN echo "NODE Version:" && node --version
RUN echo "NPM Version:" && npm --version

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY --from=node_base . .
WORKDIR /src
COPY ["HSS.IdentityServer/HSS.IdentityServer.csproj", "HSS.IdentityServer/"]
COPY ["HSS.Common/HSS.Common.csproj", "HSS.Common/"]
RUN dotnet restore "HSS.IdentityServer/HSS.IdentityServer.csproj"
COPY . .
WORKDIR "/src/HSS.IdentityServer"

RUN dotnet build "HSS.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HSS.IdentityServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HSS.IdentityServer.dll"]