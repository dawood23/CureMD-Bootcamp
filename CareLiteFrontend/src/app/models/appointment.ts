export interface Appointment {
  appointmentID: number;
  patientID:number;
  patientName: string;
  doctorID:number;
  doctorName: string;
  startTime: string;        
  durationMinutes: number;
  status: string;
  createdAt: string;        
}
