export class UserDto {
  id: number;
  firstName: string;
  lastName: string;
  isActive: boolean;

  constructor(id: number, firstName: string, lastName: string, isActive: boolean) {
    this.id = id;
    this.firstName = firstName;
    this.lastName = lastName;
    this.isActive = isActive;
  }

  public toString(): string {
    return this.getName();
  }

  public getName(): string {
    return this.firstName + ' ' + this.lastName;
  }
}
