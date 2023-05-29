export class AuthenticatedUser {
  tokenId: string;
  userName: string;
  id: string;
  memberId?: string;
  userFirst: string;
  userLast: string;
  token: string;

  constructor(token: string, tokenId: string, id: string, userName: string, firstName: string, lastName: string, memberId?: string) {
    this.token = token;
    this.tokenId = tokenId;
    this.id = id;
    this.userName = userName;
    this.memberId = memberId;
    this.userFirst = firstName;
    this.userLast = lastName;
  }
}
