import 'dotenv/config'
import { defineConfig } from 'prisma/config'

// `url` is omitted rather than passed as undefined when DATABASE_URL is unset:
// `prisma generate` must work without a database (that is how the Docker build
// generates the client), while migrate/studio still fail loudly on their own.
const url = process.env.DATABASE_URL

export default defineConfig({
  schema: 'prisma/',
  migrations: {
    path: 'prisma/migrations',
  },
  datasource: url ? { url } : {},
})
