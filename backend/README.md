## Description
TBD

## Project setup

```bash
$ npm install
```

## Compile and run the project

```bash
# development
$ npm run start
# to run db - move to root?
$ docker compose up -d postgres
$ npx prisma migrate dev
$ npx prisma studio

# watch mode
$ npm run start:dev

# production mode
$ npm run start:prod

```

## Run tests

```bash
# unit tests
$ npm run test

# e2e tests
$ npm run test:e2e

# test coverage
$ npm run test:cov
```

## Resources
- [NestJS Documentation](https://docs.nestjs.com)
- [deployment documentation](https://docs.nestjs.com/deployment)
- [NestJS Mau](https://mau.nestjs.com)

- [NestJS Devtools](https://devtools.nestjs.com)
