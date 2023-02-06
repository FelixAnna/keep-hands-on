#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EStore.CartAPI/EStore.CartAPI.csproj", "EStore.CartAPI/"]
COPY ["EStore.Common/EStore.Common.csproj", "EStore.Common/"]
COPY ["EStore.SharedServices.SqlServer/EStore.DataAccess.SqlServer.csproj", "EStore.SharedServices.SqlServer/"]
COPY ["EStore.SharedServices/EStore.SharedServices.csproj", "EStore.SharedServices/"]
COPY ["EStore.EventServices.Azure/EStore.EventServices.Azure.csproj", "EStore.EventServices.Azure/"]
RUN dotnet restore "EStore.CartAPI/EStore.CartAPI.csproj"
COPY . .
WORKDIR "/src/EStore.CartAPI"
RUN dotnet build "EStore.CartAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EStore.CartAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EStore.CartAPI.dll"]