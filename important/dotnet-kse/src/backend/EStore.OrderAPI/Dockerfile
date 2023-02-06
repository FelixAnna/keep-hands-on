#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EStore.OrderAPI/EStore.OrderAPI.csproj", "EStore.OrderAPI/"]
COPY ["EStore.Common/EStore.Common.csproj", "EStore.Common/"]
COPY ["EStore.EventServices.Azure/EStore.EventServices.Azure.csproj", "EStore.EventServices.Azure/"]
COPY ["EStore.SharedServices.SqlServer/EStore.DataAccess.SqlServer.csproj", "EStore.SharedServices.SqlServer/"]
COPY ["EStore.SharedServices/EStore.SharedServices.csproj", "EStore.SharedServices/"]
RUN dotnet restore "EStore.OrderAPI/EStore.OrderAPI.csproj"
COPY . .
WORKDIR "/src/EStore.OrderAPI"
RUN dotnet build "EStore.OrderAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EStore.OrderAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EStore.OrderAPI.dll"]