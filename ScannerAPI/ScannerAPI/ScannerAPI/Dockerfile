# File: ScannerAPI/Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY bin/ReleasePublish ./
ENTRYPOINT ["dotnet", "ScannerAPI.dll"]
