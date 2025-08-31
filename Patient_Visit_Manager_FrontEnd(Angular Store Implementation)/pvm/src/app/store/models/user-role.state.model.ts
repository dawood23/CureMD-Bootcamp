import { UserRole } from "../../models/user-roles";

export interface UserRoleStateModel {
  userRoles: UserRole[];
  loading: boolean;
  error: string | null;
}
