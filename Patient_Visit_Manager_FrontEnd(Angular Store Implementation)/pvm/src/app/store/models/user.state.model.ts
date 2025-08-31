import { User } from "../../models/user";

export interface UserStateModel {
  users: User[];
  loading: boolean;
  error: string | null;
}