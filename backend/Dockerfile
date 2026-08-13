# syntax=docker/dockerfile:1

# ---------- build stage ----------
FROM node:22-alpine AS builder

WORKDIR /app

COPY package.json package-lock.json ./

# The cache mount lets a retried build reuse already-downloaded tarballs instead
# of refetching the whole tree; --no-audit/--no-fund drop two registry round
# trips that add nothing to a build. Both matter on a slow or filtered network.
RUN --mount=type=cache,target=/root/.npm \
    npm ci --no-audit --no-fund --fetch-retries=5

# prisma.config.ts is required: the datasource url and the multi-file schema
# location (prisma/) are declared there, not in schema.prisma.
COPY tsconfig.json tsconfig.build.json nest-cli.json prisma.config.ts ./
COPY prisma ./prisma
COPY src ./src

# `prebuild` runs `prisma generate`, which writes src/generated/prisma
# inside the image. Never copy a host-generated client in.
RUN npm run build

RUN npm prune --omit=dev

# ---------- runtime stage ----------
FROM node:22-alpine

WORKDIR /app

ENV NODE_ENV=production
ENV PORT=3000

COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/package.json ./
COPY --from=builder /app/dist ./dist
# prisma/ + prisma.config.ts stay in the image so `prisma migrate deploy`
# can run at container start (the prisma CLI survives `npm prune`).
COPY --from=builder /app/prisma ./prisma
COPY --from=builder /app/prisma.config.ts ./

USER node

EXPOSE 3000
CMD ["node", "dist/main.js"]
