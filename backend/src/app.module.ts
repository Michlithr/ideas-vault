import { Module } from '@nestjs/common'
import { AppController } from './app.controller'
import { HouseholdModule } from '@modules/household/household.module'

@Module({
  imports: [HouseholdModule],
  controllers: [AppController],
})
export class AppModule { }
