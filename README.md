# Ideas Vault

A shared place for two people in one household to save ideas (places, recipes, films, gifts) so links, plans and ideas stop getting lost.

## Tech stack
- Backend: ASP.NET Core (.NET 10), EF Core 10 + Npgsql
- Frontend: React/Vue 3 + TypeScript (planned, not in the repo yet)
- Database: PostgreSQL 16
- Runtime: Docker Compose

## Prerequisites
- .NET 10 SDK
- Docker (with Compose v2)
- `dotnet-ef` CLI, 10.x: `dotnet tool install --global dotnet-ef`

## Run locally (app on host, DB in Docker)
1. `cp .env.example .env` and fill in `POSTGRES_USER`, `POSTGRES_PASSWORD`, `POSTGRES_DB`, `POSTGRES_PORT`, `BACKEND_PORT`
   (all five are required, even for local runs)
2. `docker compose up -d postgres`
3. `./backend/scripts/setup-dev-secrets.sh`  (copies the DB settings from `.env` into .NET user-secrets)
4. `dotnet ef database update --project backend/IdeasVault.Api`
5. `dotnet run --project backend/IdeasVault.Api`  (serves on http://localhost:5132)

## Run everything in Docker
1. Complete step 1 above (`.env` is required, Compose won't start without it)
2. `docker compose up --build`
3. Migrations are not applied automatically yet. From the host, run step 3 (user-secrets, the host needs DB settings too) and then step 4.

The backend is exposed on `BACKEND_PORT`. Compose supplies the DB settings to the container only, not to the host.

TODO: consider applying migrations at startup (`Database.Migrate()`) so `docker compose up` works on a
fresh clone without the manual step. Fine for dev and a single instance. For production, prefer a separate
migrate step (e.g. `dotnet ef migrations bundle`), because concurrent instances racing to migrate is unsafe.

## Project layout
- `backend/`: ASP.NET Core API, Dockerfile, dev scripts
- `frontend/`: Vue app (planned)
- `docker-compose.yml`: Postgres + backend
