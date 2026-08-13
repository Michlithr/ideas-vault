import { Module } from '@nestjs/common'
import { PrismaModule } from '@modules/prisma/prisma.module'
import { HouseholdController } from '@modules/household/household.controller'
import { HouseholdService } from '@modules/household/household.service'

@Module({
  imports: [PrismaModule],
  controllers: [HouseholdController],
  providers: [HouseholdService],
})
export class HouseholdModule {}
