#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Mango.Service.ConfigCenter/Mango.Service.ConfigCenter.csproj", "Mango.Service.ConfigCenter/"]
COPY ["Mango.Service.ConfigCenter/NuGet.config", "Mango.Service.ConfigCenter/"]
COPY ["Lib/", "Lib/"]
COPY ["Mango.Service.ConfigCenter.Abstraction/Mango.Service.ConfigCenter.Abstraction.csproj", "Mango.Service.ConfigCenter.Abstraction/"]
RUN dotnet restore "Mango.Service.ConfigCenter/Mango.Service.ConfigCenter.csproj"
COPY . .
WORKDIR "/src/Mango.Service.ConfigCenter"
RUN dotnet build "Mango.Service.ConfigCenter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mango.Service.ConfigCenter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mango.Service.ConfigCenter.dll"]