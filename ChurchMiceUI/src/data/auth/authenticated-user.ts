export class AuthenticatedUser {
  tokenId: string;
  userName: string;
  userId: string;
  memberId?: string;
  userFirst: string;
  userLast: string;
  token: string;

  constructor(token: string, tokenId: string, userId: string, userName: string, firstName: string, lastName: string, memberId?: string) {
    this.token = token;
    this.tokenId = tokenId;
    this.userId = userId;
    this.userName = userName;
    this.memberId = memberId;
    this.userFirst = firstName;
    this.userLast = lastName;
  }
}
