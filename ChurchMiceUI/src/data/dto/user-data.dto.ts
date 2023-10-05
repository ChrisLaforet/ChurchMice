export class UserDataDto {
  id: string;
  userName: string;
  fullName: string;
  email: string;
  roleLevel: string;
  createdDate: string;
  lastLoginDate: string | null;

  constructor(id: string, userName: string, fullName: string, email: string, roleLevel: string, createdDate: string, lastLoginDate: string | null) {
    this.id = id;
    this.userName = userName;
    this.fullName = fullName;
    this.email = email;
    this.roleLevel = roleLevel;
    this.createdDate = createdDate;
    this.lastLoginDate = lastLoginDate;
  }

  public toString(): string {
    return this.userName;
  }
}
