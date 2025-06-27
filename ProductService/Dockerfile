# Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

RUN dotnet tool install --global dotnet-watch

COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=build /root/.dotnet /root/.dotnet/tools
ENV PATH="${PATH}:/root/.dotnet/tools"
ENV DOTNET_WATCH_RESTART_ON_RUDE_CHANGE=true
ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile", "ProductService.dll"]
