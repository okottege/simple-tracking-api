FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["TrackerService.Api/TrackerService.Api.csproj", "TrackerService.Api/"]
COPY ["TrackerService.Common/TrackerService.Common.csproj", "TrackerService.Common/"]
COPY ["TrackerService.Data/TrackerService.Data.csproj", "TrackerService.Data/"]
RUN dotnet restore "TrackerService.Api/TrackerService.Api.csproj"
COPY . .
WORKDIR "/src/TrackerService.Api"
RUN dotnet build "TrackerService.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TrackerService.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TrackerService.Api.dll"]