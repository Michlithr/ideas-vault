import { Controller, Get, HttpStatus } from '@nestjs/common'

@Controller()
export class AppController {
  @Get('health')
  getHealth(): HttpStatus {
    return HttpStatus.OK
  }
}