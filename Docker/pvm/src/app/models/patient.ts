export interface Patient {
  patientID?: number;
  firstName: string;
  lastName: string;
  dateOfBirth?: string;
  phoneNumber?: string;
  email?: string;
  address?: string;
  emergencyContact?: string;
}