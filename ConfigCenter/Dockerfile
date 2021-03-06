#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:latest AS base
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
ENV LANG C.UTF-8
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:latest AS build
WORKDIR /src
COPY ["Mango.Service.ConfigCenter/Mango.Service.ConfigCenter.csproj", "Mango.Service.ConfigCenter/"]
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