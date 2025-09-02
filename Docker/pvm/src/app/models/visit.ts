export interface Visit {
  visitID?: number;
  patientID: number;
  doctorID?: number;
  visitTypeID: number;
  visitDate: string;
  visitTime: string;
  description?: string;
  notes?: string;
  status: string;
  fee?: number;
  createdBy?: number;
}