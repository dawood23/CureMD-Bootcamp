export interface Bill{
    billID:number,
    appointmentID:number,
    patientID:number,
    patientName:string,
    doctorName:string,
    generatedAt:Date,
    status:string,
    totalAmount:number,
    pendingAmount:number
}