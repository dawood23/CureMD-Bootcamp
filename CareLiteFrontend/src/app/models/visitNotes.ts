export interface visitNotes{
    visitNoteID:number,
    appointmenID:number,
    content:string,
    createdAt:Date,
    updatedAt:Date,
    patientID:number,
    patientName:string,
    doctorID:number,
    doctorName:string,
    appointmentStatus:string
}

export interface UpdateVisitRequest {
  content: string;
}

export interface CreateVisitRequeset{
    appointmentID:number,
    content:string
}