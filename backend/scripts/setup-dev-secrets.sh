#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/../.." && pwd)"
PROJECT="$ROOT/backend/IdeasVault.Api"

[ -f "$ROOT/.env" ] || { echo "Missing $ROOT/.env (copy .env.example first)" >&2; exit 1; }

set -a; source "$ROOT/.env"; set +a

cd "$PROJECT"
dotnet user-secrets set "Db:Host" "localhost" >/dev/null
dotnet user-secrets set "Db:Port" "$POSTGRES_PORT" >/dev/null
dotnet user-secrets set "Db:Name" "$POSTGRES_DB" >/dev/null
dotnet user-secrets set "Db:User" "$POSTGRES_USER" >/dev/null
dotnet user-secrets set "Db:Password" "$POSTGRES_PASSWORD" >/dev/null

echo "Dev secrets set for $PROJECT"