export class UserDto {
  id: number;
  fullName: string;
  isActive: boolean;

  constructor(id: number, fullName: string, isActive: boolean) {
    this.id = id;
    this.fullName = fullName;
    this.isActive = isActive;
  }

  public toString(): string {
    return this.getName();
  }

  public getName(): string {
    return this.fullName;
  }
}
