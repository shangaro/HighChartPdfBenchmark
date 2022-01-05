FROM mcr.microsoft.com/dotnet/sdk:latest AS base
WORKDIR /app

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["HighChartPdfBenchmark/HighChartPdfBenchmark.csproj", "HighChartPdfBenchmark/"]
RUN dotnet restore "HighChartPdfBenchmark/HighChartPdfBenchmark.csproj"
COPY . .
WORKDIR "/src/HighChartPdfBenchmark"
RUN dotnet build "HighChartPdfBenchmark.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HighChartPdfBenchmark.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HighChartPdfBenchmark.dll"]
