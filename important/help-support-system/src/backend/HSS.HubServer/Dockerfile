#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HSS.HubServer/HSS.HubServer.csproj", "HSS.HubServer/"]
COPY ["HSS.Common/HSS.Common.csproj", "HSS.Common/"]
RUN dotnet restore "HSS.HubServer/HSS.HubServer.csproj"
COPY . .
WORKDIR "/src/HSS.HubServer"
RUN dotnet build "HSS.HubServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HSS.HubServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HSS.HubServer.dll"]