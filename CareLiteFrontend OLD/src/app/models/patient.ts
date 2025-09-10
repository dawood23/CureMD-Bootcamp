export interface Patient {
  patientID: number;
  firstName: string;
  lastName: string;
  dob?: Date | null;
  gender?: string | null;
  phone?: string | null;
  email?: string | null;
  address?: string | null;
  createdAt: Date;
}
