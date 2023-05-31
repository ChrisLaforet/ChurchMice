export class AuthenticatedUser {
  tokenId: string;
  userName: string;
  id: string;
  fullName: string;
  token: string;

  constructor(token: string, tokenId: string, id: string, userName: string, fullName: string) {
    this.token = token;
    this.tokenId = tokenId;
    this.id = id;
    this.userName = userName;
    this.fullName = fullName;
  }
}
