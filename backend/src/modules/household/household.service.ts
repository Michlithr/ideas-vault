import { Injectable, NotFoundException, ConflictException } from '@nestjs/common'
import { PrismaService } from '@modules/prisma/prisma.service'
import { type Household, Prisma } from '@prisma-client'
import { CreateHouseholdDto } from '@modules/household/dto/createHousehold.dto'
import { PrismaErrorCodes } from '@consts/PrismaErrorCodes'

@Injectable()
export class HouseholdService {
  constructor(private readonly prisma: PrismaService) { }

  async findById(id: string): Promise<Household> {
    const household = await this.prisma.household.findUnique({
      where: { id },
    })

    if (!household) throw new NotFoundException(`Household with id "${id}" was not found`)

    return household
  }

  async createHousehold(dto: CreateHouseholdDto): Promise<Household> {
    try {
      return await this.prisma.household.create({
        data: { name: dto.name },
      })
    } catch (error) {
      this.handleCreateHouseholdError(error, dto.name)
    }
  }

  private handleCreateHouseholdError(error: unknown, householdName: string): never {
    if (
      error instanceof Prisma.PrismaClientKnownRequestError &&
      error.code === PrismaErrorCodes.UNIQUE_CONSTRAINT_ERROR
    ) {
      throw new ConflictException(`Household with name "${householdName}" already exists`)
    }

    throw error
  }
}
