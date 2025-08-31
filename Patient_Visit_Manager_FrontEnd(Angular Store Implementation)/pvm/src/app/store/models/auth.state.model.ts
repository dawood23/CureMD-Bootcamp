export interface AuthStateModel {
  token: string | null;
  isAuthenticated: boolean;
  currentUser: any | null;
  userRole: string | null;
  userId: number | null;
  loading: boolean;
  error: string | null;
}
