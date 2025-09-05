export interface User {
  userID?: number;     
  username: string;
  passwordHash: string;
  email:string,
  phone:string,
  roleID: number;
  active: boolean;
}
