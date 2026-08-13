import { Body, Controller, Get, Param, Post, HttpCode, HttpStatus } from '@nestjs/common'
import { HouseholdService } from './household.service'
import { CreateHouseholdDto } from './dto/createHousehold.dto'
import type { Household } from '@prisma-client'

@Controller('households')
export class HouseholdController {
  constructor(private readonly householdService: HouseholdService) {}

  @Get(':id')
  async findById(@Param('id') id: string): Promise<Household> {
    return this.householdService.findById(id)
  }

  @Post()
  @HttpCode(HttpStatus.CREATED)
  async create(@Body() createHouseholdDto: CreateHouseholdDto): Promise<Household> {
    return this.householdService.createHousehold(createHouseholdDto)
  }
}
