export interface User {
  userID?: number;
  username: string;
  passwordHash?: string;
  roleID: number;
  firstName: string;
  lastName: string;
}
