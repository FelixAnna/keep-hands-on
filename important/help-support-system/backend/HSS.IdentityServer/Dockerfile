#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HSS.IdentityServer/HSS.IdentityServer.csproj", "HSS.IdentityServer/"]
COPY ["HSS.Common/HSS.Common.csproj", "HSS.Common/"]
RUN dotnet restore "HSS.IdentityServer/HSS.IdentityServer.csproj"
COPY . .
WORKDIR "/src/HSS.IdentityServer"
RUN apt-get install -y curl \
  && curl -sL https://deb.nodesource.com/setup_18.x | bash - \
  && apt-get install -y nodejs \
  && curl -L https://www.npmjs.com/install.sh | sh

RUN dotnet build "HSS.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HSS.IdentityServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HSS.IdentityServer.dll"]