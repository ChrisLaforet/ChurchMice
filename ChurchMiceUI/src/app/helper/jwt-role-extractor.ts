import jwt_decode from 'jwt-decode';

export class JwtRoleExtractor {

  public static getRoles(token: string): string[] {
    try {
      const tokenPayload: object = jwt_decode(token);
      // @ts-ignore
      const expiration = parseInt(tokenPayload.exp, 10);
      const now = new Date();
      // @ts-ignore
      if (now.getTime() / 1000 > expiration) {
        console.log('User token is expired');
        return [];
      }
      // @ts-ignore
      const roles: string[] = tokenPayload['roles'];
      if (roles !== undefined) {
        return roles;
      }
    } catch (e) {
      console.error('getRoles - Invalid token format being validated: ' + e);
    }
    return [];
  }

  public static isUserAnAdministrator(token: string): boolean {
    try {
      const tokenPayload: object = jwt_decode(token);
      // @ts-ignore
      const expiration = parseInt(tokenPayload.exp, 10);
      const now = new Date();
      // @ts-ignore
      if (now.getTime() / 1000 > expiration) {
        console.log('User token is expired');
        return false;
      }
      // @ts-ignore
      const roles: string[] = tokenPayload['roles'];
      if (roles === undefined) {
        return false;
      }

      return roles.includes('Administrator');

    } catch (e) {
      console.error('isUserAnAdministrator - Invalid token format being validated: ' + e);
      return false;
    }
  }
}
