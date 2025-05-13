# Scripts/Database/migrate-database.sh
#!/usr/bin/env bash
# Aplica migraciones pendientes a la base de datos
pushd ScannerAPI/ScannerAPI || exit 1
dotnet ef database update
popd