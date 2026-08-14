import { Test } from '@nestjs/testing'
import { HttpStatus } from '@nestjs/common'
import { AppController } from './app.controller'

describe('AppController', () => {
  let appController: AppController

  beforeEach(async () => {
    const app = await Test.createTestingModule({
      controllers: [AppController],
    }).compile()

    appController = app.get(AppController)
  })

  describe('health', () => {
    it('should return OK status', () => {
      expect(appController.getHealth()).toBe(HttpStatus.OK)
    })
  })
})
