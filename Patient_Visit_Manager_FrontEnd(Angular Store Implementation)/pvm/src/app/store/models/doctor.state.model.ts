import { Doctor } from "../../models/doctor";

export interface DoctorStateModel {
  doctors: Doctor[];
  loading: boolean;
  error: string | null;
}