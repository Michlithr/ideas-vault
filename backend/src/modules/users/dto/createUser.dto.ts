import { IsString, IsNotEmpty, Matches, MinLength, MaxLength, IsOptional } from 'class-validator'

export class CreateUserDto {
  @IsString({ message: 'Username have to be a string' })
  @MinLength(3, { message: 'Username have to contain at least 3 chars' })
  @MaxLength(50, { message: 'Username can not be longer than 50 chars' })
  @Matches(/^[a-zA-Z0-9_-]+$/)
  username!: string

  @IsString({ message: 'Password have to be a string' })
  @MinLength(12, { message: 'Password have to contain at least 12 chars' })
  @MaxLength(72, { message: 'Password can not be longer than 72 chars' })
  password!: string

  @IsString({ message: 'Household id have to be a string' })
  @IsNotEmpty({ message: 'Household id can not be empty' })
  householdId!: string

  @IsOptional()
  @IsString({ message: 'Display name have to be a string' })
  @MinLength(3, { message: 'Display name have to contain at least 3 chars' })
  @MaxLength(50, { message: 'Display name can not be longer than 50 chars' })
  displayName?: string
}
