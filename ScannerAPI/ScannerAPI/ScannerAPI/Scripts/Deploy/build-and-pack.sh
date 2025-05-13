# Scripts/Deploy/build-and-pack.sh
#!/usr/bin/env bash
# Compila y empaqueta la aplicación para producción
dotnet publish ScannerAPI/ScannerAPI.csproj -c Release -o bin/ReleasePublish