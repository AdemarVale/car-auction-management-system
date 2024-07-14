#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG VERSION=1.0.0
ARG ENVIRONMENT=staging

WORKDIR /src
COPY ["src/CarAuctionManagementSystem.Api/CarAuctionManagementSystem.Api.csproj", "src/CarAuctionManagementSystem.Api/"]
COPY ["src/CarAuctionManagementSystem.Application/CarAuctionManagementSystem.Application.csproj", "src/CarAuctionManagementSystem.Application/"]
COPY ["src/CarAuctionManagementSystem.Domain/CarAuctionManagementSystem.Domain.csproj", "src/CarAuctionManagementSystem.Domain/"]
COPY ["src/CarAuctionManagementSystem.Infrastructure/CarAuctionManagementSystem.Infrastructure.csproj", "src/CarAuctionManagementSystem.Infrastructure/"]
COPY ["src/CarAuctionManagementSystem.Persistence/CarAuctionManagementSystem.Persistence.csproj", "src/CarAuctionManagementSystem.Persistence/"]
RUN dotnet restore "src/CarAuctionManagementSystem.Api/CarAuctionManagementSystem.Api.csproj"
COPY . .
WORKDIR "/src/src/CarAuctionManagementSystem.Api"
RUN echo "The version to build is $VERSION"
RUN dotnet build "CarAuctionManagementSystem.Api.csproj" /p:Version=$VERSION -c Release -o /app/build

FROM build AS publish
RUN echo "The version to publish is $VERSION"
RUN dotnet publish "CarAuctionManagementSystem.Api.csproj" /p:Version=$VERSION -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=$ENVIRONMENT
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarAuctionManagementSystem.Api.dll"]