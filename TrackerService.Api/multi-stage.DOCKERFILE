FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build-env
WORKDIR /app

COPY ["TrackerService.Api/TrackerService.Api.csproj", "TrackerService.Api/"]
COPY ["TrackerService.Common/TrackerService.Common.csproj", "TrackerService.Common/"]
COPY ["TrackerService.Data/TrackerService.Data.csproj", "TrackerService.Data/"]
COPY ["TrackerService.Data/TrackerService.Core.csproj", "TrackerService.Core/"]
COPY ["TrackerService.Data/TrackerService.Services.csproj", "TrackerService.Services/"]

RUN dotnet restore "TrackerService.Api/TrackerService.Api.csproj"

COPY . .

RUN dotnet publish "TrackerService.Api/TrackerService.Api.csproj" -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "TrackerService.Api.dll"]