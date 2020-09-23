#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Common.WebApi/Common.WebApi.csproj", "Common.WebApi/"]
RUN dotnet restore "Common.WebApi/Common.WebApi.csproj"
COPY . .
WORKDIR "/src/Common.WebApi"
RUN dotnet build "Common.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Common.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Common.WebApi.dll"]