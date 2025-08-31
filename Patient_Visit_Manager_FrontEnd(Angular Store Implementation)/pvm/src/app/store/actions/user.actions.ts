import { User } from "../../models/user";

export class LoadUsers {
  static readonly type = '[User] Load Users';
}

export class LoadUsersSuccess {
  static readonly type = '[User] Load Users Success';
  constructor(public payload: User[]) {}
}

export class LoadUsersFailure {
  static readonly type = '[User] Load Users Failure';
  constructor(public payload: string) {}
}

export class CreateUser {
  static readonly type = '[User] Create User';
  constructor(public payload: User) {}
}

export class CreateUserSuccess {
  static readonly type = '[User] Create User Success';
  constructor(public payload: User) {}
}

export class CreateUserFailure {
  static readonly type = '[User] Create User Failure';
  constructor(public payload: string) {}
}

export class UpdateUser {
  static readonly type = '[User] Update User';
  constructor(public payload: { id: number; user: User }) {}
}

export class UpdateUserSuccess {
  static readonly type = '[User] Update User Success';
  constructor(public payload: User) {}
}

export class UpdateUserFailure {
  static readonly type = '[User] Update User Failure';
  constructor(public payload: string) {}
}

export class DeleteUser {
  static readonly type = '[User] Delete User';
  constructor(public payload: number) {}
}

export class DeleteUserSuccess {
  static readonly type = '[User] Delete User Success';
  constructor(public payload: number) {}
}

export class DeleteUserFailure {
  static readonly type = '[User] Delete User Failure';
  constructor(public payload: string) {}
}