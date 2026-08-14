import { Test } from '@nestjs/testing'
import { HouseholdController } from './household.controller'
import { HouseholdService } from './household.service'
import type { Household } from '@prisma-client'

describe('HouseholdController', () => {
  let controller: HouseholdController
  let service: { findById: jest.Mock; createHousehold: jest.Mock }

  const household: Household = {
    id: 'household_1',
    name: 'Kowalski Household',
    updatedAt: new Date(),
  }

  beforeEach(async () => {
    service = {
      findById: jest.fn(),
      createHousehold: jest.fn(),
    }

    const module = await Test.createTestingModule({
      controllers: [HouseholdController],
      providers: [{ provide: HouseholdService, useValue: service }],
    }).compile()

    controller = module.get(HouseholdController)
  })

  describe('findById', () => {
    it('delegates to the service with the route param', async () => {
      service.findById.mockResolvedValue(household)

      await expect(controller.findById(household.id)).resolves.toEqual(household)
      expect(service.findById).toHaveBeenCalledWith(household.id)
    })
  })

  describe('create', () => {
    it('delegates to the service with the request body', async () => {
      service.createHousehold.mockResolvedValue(household)

      await expect(controller.create({ name: household.name })).resolves.toEqual(household)
      expect(service.createHousehold).toHaveBeenCalledWith({ name: household.name })
    })
  })
})
