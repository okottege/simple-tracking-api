FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build-env
WORKDIR /app
COPY ["TrackerService.Api/TrackerService.Api.csproj", "TrackerService.Api/"]
COPY ["TrackerService.Common/TrackerService.Common.csproj", "TrackerService.Common/"]
COPY ["TrackerService.Data/TrackerService.Data.csproj", "TrackerService.Data/"]
COPY ["Tests/Tracker.Api.Tests/Tracker.Api.Tests.csproj", "Tests/Tracker.Api.Tests/"]
RUN dotnet restore "TrackerService.Api/TrackerService.Api.csproj"
RUN dotnet restore "Tests/Tracker.Api.Tests/Tracker.Api.Tests.csproj"
COPY . .
RUN dotnet test "Tests/Tracker.Api.Tests/Tracker.Api.Tests.csproj" --logger:trx
RUN dotnet publish "TrackerService.Api/TrackerService.Api.csproj" -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "TrackerService.Api.dll"]