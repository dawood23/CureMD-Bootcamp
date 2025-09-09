export interface AppointmentRequest {
  appointmentID:number,
  patientID: number;
  doctorID: number;
  createdBy: number;
  startTime: string;        
  durationMinutes: number;  
  status?: string;         
}
