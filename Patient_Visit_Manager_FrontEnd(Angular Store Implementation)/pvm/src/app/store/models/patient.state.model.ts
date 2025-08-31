import { Patient } from "../../models/patient";

export interface PatientStateModel {
  patients: Patient[];
  loading: boolean;
  error: string | null;
}
