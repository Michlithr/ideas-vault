import * as bcrypt from 'bcrypt';

import { Injectable, NotFoundException, ConflictException, BadRequestException } from "@nestjs/common";
import { PrismaService } from "@modules/prisma/prisma.service";
import { User, Prisma } from '@prisma-client'
import { CreateUserDto } from "@modules/users/dto/createUser.dto";

import { SALT_ROUND } from './consts'
import { PrismaErrorCodes } from '@consts/PrismaErrorCodes'

@Injectable()
export class UsersService {
  constructor(private readonly prisma: PrismaService) { }

  async findById(id: string): Promise<Omit<User, 'password'>> {
    return await this.findUnique({ id })
  }

  async findByUsername(username: string): Promise<Omit<User, 'password'>> {
    return await this.findUnique({ username })
  }

  private async findUnique(where: Prisma.UserWhereUniqueInput): Promise<User> {
    const user = await this.prisma.user.findUnique({ where })

    if (!user) throw new NotFoundException(`User was not found`)

    return user
  }

  async createUser(dto: CreateUserDto): Promise<Omit<User, 'password'>> {
    const passwordHash = await bcrypt.hash(dto.password, SALT_ROUND)
    try {
      const { password, ...user } = await this.prisma.user.create({
        data: {
          username: dto.username,
          password: passwordHash,
          householdId: dto.householdId,
          displayName: dto.displayName ?? null
        }
      })
      return user
    } catch (error) {
      this.handleCreateUserError(error)
    }
  }

  private handleCreateUserError(error: unknown): never {
    if (!(error instanceof Prisma.PrismaClientKnownRequestError))
      throw error
    if (error.code === PrismaErrorCodes.UNIQUE_CONSTRAINT_ERROR)
      throw new ConflictException(`User already exists`)
    if (error.code === PrismaErrorCodes.UNKNOWN_FOREIGN_KEY)
      throw new BadRequestException(`Household not found`)
    throw error
  }
}
