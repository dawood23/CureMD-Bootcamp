export interface Payment {
  paymentID: number;
  billID: number;
  appointmentID: number;
  doctorID: number;
  doctorName: string;
  patientID: number;
  patientName: string;
  amount: number;
  method: string;
  paidAt: Date;
  recordedBy: number;
}