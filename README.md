# ideas-vault

NestJS + Prisma 7 + Postgres.

## Prerequisites

- Node **22.12+** (`nvm use` picks it up from [.nvmrc](.nvmrc)). Prisma 7 refuses to
  install on anything older — `npm ci` fails outright on e.g. 22.5.
- Docker (for Postgres).

## Local development

**Only Postgres runs in Docker. The app runs directly on your host.** That is
deliberate — it is the difference between a sub-second edit-reload cycle and a
30-second image rebuild:

| | App on host (this setup) | App in a container |
| --- | --- | --- |
| Reload after an edit | ~1s, `nest start --watch` recompiles the changed file | full `docker compose build` unless you mount sources and re-plumb watching |
| Debugging | attach a debugger straight to the node process, breakpoints in `.ts` | requires exposing an inspector port and path mapping into the container |
| Stack traces | point at your real source files | point at paths inside the image |
| Postgres | still isolated and disposable in Docker | same |

The database belongs in Docker because it is stateful, version-pinned, and you
never edit it — the exact opposite of application code.

### First run

```bash
cp .env.example .env                  # compose values (Postgres credentials, ports)
cp backend/.env.example backend/.env  # app + Prisma CLI values (localhost DB)

nvm use                               # Node 22.12+, from .nvmrc
docker compose up -d postgres         # only the database

cd backend
npm ci
npm run db:migrate                    # apply migrations (creates the schema)
npm run start:dev                     # http://localhost:3000
```

Note the two `.env` files and why they differ: the container reaches Postgres at
host `postgres` (compose network), while your host process reaches it at
`localhost` through the published port. Keep `backend/.env` on `localhost`.

### The day-to-day loop

```bash
docker compose up -d postgres   # once per boot; data survives in a named volume
cd backend && npm run start:dev # leave running — it recompiles on every save
```

`start:dev` is `nest start --watch`: it type-checks and restarts on change, and
`prisma generate` runs before it so the client is never stale after a schema
edit. Watch mode does **not** pick up new migrations — after editing anything in
`prisma/`, run `npm run db:migrate` in a second terminal.

Useful while the loop is running:

```bash
npm run db:studio               # browse/edit rows in the browser
docker compose logs -f postgres # database logs
docker compose down             # stop the DB (data kept)
docker compose down -v          # stop and wipe the data volume
```

### Debugging with breakpoints

```bash
npm run start:debug             # nest start --debug --watch, inspector on :9229
```

Then attach your editor. `sourceMap: true` is set, so breakpoints land in your
`.ts` files rather than compiled output. For VS Code, create
`.vscode/launch.json` (gitignored, so it stays personal):

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "type": "node",
      "request": "attach",
      "name": "Attach to backend",
      "port": 9229,
      "restart": true,
      "cwd": "${workspaceFolder}/backend",
      "skipFiles": ["<node_internals>/**"]
    }
  ]
}
```

`"restart": true` matters: watch mode restarts the process on every save, and
without it the debugger detaches after the first change. For a
breakpoint on startup code use `node --inspect-brk` via `npm run test:debug`
(jest) or attach before the first request.

### Tests

```bash
npm test          # unit
npm run test:e2e  # boots the Nest app in-process
npm run test:cov  # coverage
```

Neither suite needs a running database as long as it does not query one —
Prisma's connection is lazy.

### Prisma cheatsheet

| Command | What it does |
| --- | --- |
| `npm run db:migrate` | Create + apply a migration in development |
| `npm run db:deploy` | Apply existing migrations (used by the container) |
| `npm run db:generate` | Regenerate the client into `src/generated/prisma` |
| `npm run db:studio` | Browse the data |

Schema is split across [backend/prisma/models/](backend/prisma/models/); the
datasource URL and that folder location come from
[backend/prisma.config.ts](backend/prisma.config.ts), *not* from `schema.prisma`.

## Running the whole stack in Docker

This builds the production image — use it to verify a prod-like run, not as your
edit loop (no source mounts, no watch mode):

```bash
docker compose up --build
```

The backend container applies pending migrations, then starts.

## Notes for future changes

- The generated Prisma client is TypeScript and lives in `src/generated/prisma`
  so it is compiled into `dist` with everything else. Import it via the
  `@prisma-client` alias — **never** from `@prisma/client`, which is the legacy
  package shim and fails at runtime with `Cannot find module '.prisma/client/default'`.
- Path aliases (`@prisma-client`, `@modules/*`, `@enums/*`) must point at files
  under `src/`. The Nest CLI rewrites those to relative requires at build time,
  which is why no runtime path-resolution hook is needed. An alias pointing
  outside `src/` type-checks fine and then breaks at runtime.
- `moduleFormat = "cjs"` and `importFileExtension = ""` in the generator are both
  required while the app compiles to CommonJS — without them the generated client
  is ESM-flavoured and its internal imports fail to resolve under Node and jest.
- Jest runs with `--experimental-vm-modules` because the Prisma client loads its
  WASM query compiler through a dynamic `import()`, which jest's CJS VM otherwise
  rejects. Don't simplify the `test` script back to plain `jest`.
