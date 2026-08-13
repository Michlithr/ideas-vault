import { IsString, IsNotEmpty, MinLength, MaxLength } from 'class-validator'

export class CreateHouseholdDto {
  @IsString({ message: 'Household name have to be a string' })
  @IsNotEmpty({ message: 'Household name can not be empty' })
  @MinLength(3, { message: 'Household name have to contain at least 3 chars' })
  @MaxLength(50, { message: 'Household name can not be longer than 50 chars' })
  name!: string
}
