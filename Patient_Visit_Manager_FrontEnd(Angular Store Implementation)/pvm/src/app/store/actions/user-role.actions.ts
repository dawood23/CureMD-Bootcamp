import { UserRole } from "../../models/user-roles";

export class LoadUserRoles {
  static readonly type = '[UserRole] Load User Roles';
}

export class LoadUserRolesSuccess {
  static readonly type = '[UserRole] Load User Roles Success';
  constructor(public payload: UserRole[]) {}
}

export class LoadUserRolesFailure {
  static readonly type = '[UserRole] Load User Roles Failure';
  constructor(public payload: string) {}
}

export class CreateUserRole {
  static readonly type = '[UserRole] Create User Role';
  constructor(public payload: UserRole) {}
}

export class CreateUserRoleSuccess {
  static readonly type = '[UserRole] Create User Role Success';
  constructor(public payload: UserRole) {}
}

export class CreateUserRoleFailure {
  static readonly type = '[UserRole] Create User Role Failure';
  constructor(public payload: string) {}
}

export class UpdateUserRole {
  static readonly type = '[UserRole] Update User Role';
  constructor(public payload: UserRole) {}
}

export class UpdateUserRoleSuccess {
  static readonly type = '[UserRole] Update User Role Success';
  constructor(public payload: UserRole) {}
}

export class UpdateUserRoleFailure {
  static readonly type = '[UserRole] Update User Role Failure';
  constructor(public payload: string) {}
}

export class DeleteUserRole {
  static readonly type = '[UserRole] Delete User Role';
  constructor(public payload: number) {}
}

export class DeleteUserRoleSuccess {
  static readonly type = '[UserRole] Delete User Role Success';
  constructor(public payload: number) {}
}

export class DeleteUserRoleFailure {
  static readonly type = '[UserRole] Delete User Role Failure';
  constructor(public payload: string) {}
}