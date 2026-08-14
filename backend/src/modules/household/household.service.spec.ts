import { Test } from '@nestjs/testing'
import { NotFoundException, ConflictException } from '@nestjs/common'
import { HouseholdService } from './household.service'
import { PrismaService } from '@modules/prisma/prisma.service'
import { Prisma, type Household } from '@prisma-client'
import { PrismaErrorCodes } from '@consts/PrismaErrorCodes'

describe('HouseholdService', () => {
  let service: HouseholdService
  let prisma: { household: { findUnique: jest.Mock; create: jest.Mock } }

  const household: Household = {
    id: 'household_1',
    name: 'Kowalski Household',
    updatedAt: new Date(),
  }

  beforeEach(async () => {
    prisma = {
      household: {
        findUnique: jest.fn(),
        create: jest.fn(),
      },
    }

    const module = await Test.createTestingModule({
      providers: [HouseholdService, { provide: PrismaService, useValue: prisma }],
    }).compile()

    service = module.get(HouseholdService)
  })

  describe('findById', () => {
    it('returns the household when it exists', async () => {
      prisma.household.findUnique.mockResolvedValue(household)

      await expect(service.findById(household.id)).resolves.toEqual(household)
      expect(prisma.household.findUnique).toHaveBeenCalledWith({ where: { id: household.id } })
    })

    it('throws NotFoundException when the household does not exist', async () => {
      prisma.household.findUnique.mockResolvedValue(null)

      await expect(service.findById('missing_id')).rejects.toBeInstanceOf(NotFoundException)
    })
  })

  describe('createHousehold', () => {
    it('creates and returns the household', async () => {
      prisma.household.create.mockResolvedValue(household)

      await expect(service.createHousehold({ name: household.name })).resolves.toEqual(household)
      expect(prisma.household.create).toHaveBeenCalledWith({ data: { name: household.name } })
    })

    it('throws ConflictException when the name is already taken', async () => {
      const prismaError = new Prisma.PrismaClientKnownRequestError('Unique constraint failed', {
        code: PrismaErrorCodes.UNIQUE_CONSTRAINT_ERROR,
        clientVersion: '7.9.1',
      })
      prisma.household.create.mockRejectedValue(prismaError)

      await expect(service.createHousehold({ name: household.name })).rejects.toBeInstanceOf(
        ConflictException,
      )
    })

    it('rethrows unexpected errors', async () => {
      const unexpectedError = new Error('connection lost')
      prisma.household.create.mockRejectedValue(unexpectedError)

      await expect(service.createHousehold({ name: household.name })).rejects.toBe(unexpectedError)
    })
  })
})
