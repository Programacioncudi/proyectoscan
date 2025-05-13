# Scripts/Database/seed-data.sh
#!/usr/bin/env bash
# Ejecuta semilla de datos iniciales
pushd ScannerAPI/ScannerAPI || exit 1
dotnet run -- seed-data
popd