#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HSS.UserApi/HSS.UserApi.csproj", "HSS.UserApi/"]
RUN dotnet restore "HSS.UserApi/HSS.UserApi.csproj"
COPY . .
WORKDIR "/src/HSS.UserApi"
RUN dotnet build "HSS.UserApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HSS.UserApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HSS.UserApi.dll"]