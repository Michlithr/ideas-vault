import { PrismaPg } from '@prisma/adapter-pg'
import { PrismaService } from './prisma.service'

jest.mock('@prisma/adapter-pg')
jest.mock('@prisma-client', () => ({
  PrismaClient: class {
    $connect = jest.fn().mockResolvedValue(undefined)
    $disconnect = jest.fn().mockResolvedValue(undefined)
  },
}))

describe('PrismaService', () => {
  const originalDatabaseUrl = process.env.DATABASE_URL

  afterEach(() => {
    process.env.DATABASE_URL = originalDatabaseUrl
    jest.clearAllMocks()
  })

  it('throws when DATABASE_URL is not set', () => {
    delete process.env.DATABASE_URL

    expect(() => new PrismaService()).toThrow('DATABASE_URL environment variable is not set')
  })

  it('configures the adapter with the connection string', () => {
    process.env.DATABASE_URL = 'postgresql://localhost:5432/test'

    new PrismaService()

    expect(PrismaPg).toHaveBeenCalledWith({ connectionString: 'postgresql://localhost:5432/test' })
  })

  it('connects on module init', async () => {
    process.env.DATABASE_URL = 'postgresql://localhost:5432/test'
    const service = new PrismaService()
    const connect = jest.spyOn(service, '$connect')

    await service.onModuleInit()

    expect(connect).toHaveBeenCalled()
  })

  it('disconnects on module destroy', async () => {
    process.env.DATABASE_URL = 'postgresql://localhost:5432/test'
    const service = new PrismaService()
    const disconnect = jest.spyOn(service, '$disconnect')

    await service.onModuleDestroy()

    expect(disconnect).toHaveBeenCalled()
  })
})
