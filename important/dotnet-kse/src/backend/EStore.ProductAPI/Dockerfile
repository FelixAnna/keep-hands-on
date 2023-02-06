#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EStore.ProductAPI/EStore.ProductAPI.csproj", "EStore.ProductAPI/"]
COPY ["EStore.Common/EStore.Common.csproj", "EStore.Common/"]
COPY ["EStore.DataAccess.InMemoryCache/EStore.DataAccess.MemCache.csproj", "EStore.DataAccess.InMemoryCache/"]
COPY ["EStore.SharedServices/EStore.SharedServices.csproj", "EStore.SharedServices/"]
COPY ["EStore.EventServices.Azure/EStore.EventServices.Azure.csproj", "EStore.EventServices.Azure/"]
COPY ["EStore.DataAccess.Wrapper/EStore.DataAccess.Wrapper.csproj", "EStore.DataAccess.Wrapper/"]
COPY ["EStore.SharedServices.SqlServer/EStore.DataAccess.SqlServer.csproj", "EStore.SharedServices.SqlServer/"]
RUN dotnet restore "EStore.ProductAPI/EStore.ProductAPI.csproj"
COPY . .
WORKDIR "/src/EStore.ProductAPI"
RUN dotnet build "EStore.ProductAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EStore.ProductAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EStore.ProductAPI.dll"]