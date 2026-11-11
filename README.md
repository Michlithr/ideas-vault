# Ideas vault 2
A personal knowledge/ideas management app. The backend is being migrated from NestJS to .NET Core.

## Tech stack
- Backend: .NET Core / ASP.NET Core
- Frontend: React.js with TS
- Database: Postgres

### Local development
1. `cp .env.example .env` and fill in values
2. `docker compose up -d postgres`
3. `./backend/scripts/setup-dev-secrets.sh` (sets backend user-secrets from `.env`)
4. `cd backend/IdeasVault.Api && dotnet ef database update`
