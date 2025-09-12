import { Injectable } from '@angular/core';
import { URL } from '../../Environment/env';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Role } from '../../models/roles';
import { inject } from '@angular/core';
import { Patient } from '../../models/patient';
import { Appointment } from '../../models/appointment';
import { AppointmentRequest } from '../../models/appointmentRequest';
import { doctor } from '../../models/doctor';
import { CalendarAppointment } from '../../models/calendar-models';
import { CreateVisitRequeset, UpdateVisitRequest, visitNotes } from '../../models/visitNotes';
import { Bill } from '../../models/bill';
import { PaymentRequest } from '../../models/payment-request';
import { Payment } from '../../models/payments';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  http=inject(HttpClient)
  
  getAllRoles():Observable<Role[]>{
    return this.http.get<Role[]>(`${URL.API_BASE}/roles`)
  }

  //patients
  createPatient(patient: Patient): Observable<any> {
    return this.http.post(`${URL.API_BASE}/patients/create`, patient);
  }

  getPatientById(id: number): Observable<Patient> {
    return this.http.get<Patient>(`${URL.API_BASE}/patients/${id}`);
  }

  getAllPatients(): Observable<Patient[]> {
    return this.http.get<Patient[]>(`${URL.API_BASE}/patients`);
  }

  updatePatient(patient: Patient): Observable<any> {
    return this.http.put(`${URL.API_BASE}/patients/update`, patient);
  }

  deletePatient(id: number): Observable<any> {
    return this.http.delete(`${URL.API_BASE}/patients/delete/${id}`);
  }

  //Appointments
  getAppointments():Observable<Appointment[]>{
      return this.http.get<{success:boolean,data:Appointment[]}>(`${URL.API_BASE}/appointments`).pipe(map(response=>response.data));
  }
  
  getAppointmentByID(id:number):Observable<Appointment>{
    return this.http.get<{success:boolean,data:Appointment}>(`${URL.API_BASE}/appointments/${id}`).pipe(map(response=>response.data))
  }
  AddAppointment(appointment:AppointmentRequest):Observable<any>{
    return this.http.post(`${URL.API_BASE}/appointments/create`,appointment)
  }

  updateAppointment(appointment:AppointmentRequest):Observable<any>{
    return this.http.put(`${URL.API_BASE}/appointments`,appointment)
  }

  deleteAppointment(id:number):Observable<any>{
     return this.http.delete(`${URL.API_BASE}/appointments/${id}`)
  }

  //doctors
  getDoctors():Observable<doctor[]>{
    return this.http.get<doctor[]>(`${URL.API_BASE}/doctors`)
  }

  //slots
  getWeeklyCalendar(doctorId: number, weekStartDate: string): Observable<CalendarAppointment[]> {
  return this.http.get<CalendarAppointment[]>(
    `${URL.API_BASE}/Appointments/weekly-calendar/${doctorId}/${weekStartDate}`
  );
  }
  //VisitNotes
  getVisits(): Observable<visitNotes[]> {
    return this.http.get<{success:boolean,data:visitNotes[]}>(`${URL.API_BASE}/VisitNote`).pipe(map(response=>response.data));
  }

  getVisitById(id: number): Observable<visitNotes> {
    return this.http.get<{successs:boolean,data:visitNotes}>(`${URL.API_BASE}/VisitNote/${id}`).pipe(map(response=>response.data));
  }

  updateVisit(id: number, request: UpdateVisitRequest): Observable<any> {
    return this.http.put(`${URL.API_BASE}/VisitNote/${id}`, request);
  }

  addVisit(request: CreateVisitRequeset): Observable<any> {
    return this.http.post(`${URL.API_BASE}/VisitNote`, request);
  }

  deleteVisit(id: number): Observable<any> {
    return this.http.delete(`${URL.API_BASE}/VisitNote/${id}`);
  }
  
  //Billing
  generateBill(id:number):Observable<any>{
    return this.http.post(`${URL.API_BASE}/billing/generate/${id}`,{})
  }

  getBills(): Observable<Bill[]> {
    return this.http.get<Bill[]>(`${URL.API_BASE}/billing`);
  }

   recordPayment(request: PaymentRequest): Observable<any> {
    return this.http.post<any>(`${URL.API_BASE}/billing/record`, request);
  }

  getPayment():Observable<Payment>{
    return this.http.get<Payment>(`${URL.API_BASE}/billing/payments`)
  }
}
