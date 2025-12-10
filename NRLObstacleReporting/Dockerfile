FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

#install better shell
RUN apt update
RUN apt install fish -y

RUN groupadd -g 1000 appgroup && \
    useradd --uid 1000 --gid appgroup --home-dir /app --create-home appuser

WORKDIR /app

RUN mkdir "DataProtection-Keys" #used by app, requires write access

#give owner and group r-x to /app and rwx to dataprotectionkeys
RUN chown -R appuser:appgroup /app && \
    chmod -R 550 /app && \
    chmod -R 770 /app/DataProtection-Keys 

EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NRLObstacleReporting/NRLObstacleReporting.csproj", "NRLObstacleReporting/"]
RUN dotnet restore "NRLObstacleReporting/NRLObstacleReporting.csproj"
COPY . .
WORKDIR "/src/NRLObstacleReporting"
RUN dotnet build "NRLObstacleReporting.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NRLObstacleReporting.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

USER appuser

ENTRYPOINT ["dotnet", "NRLObstacleReporting.dll"]