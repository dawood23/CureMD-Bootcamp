import { Injectable } from '@angular/core';
import { URL } from '../../Environment/env';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Role } from '../../models/roles';
import { inject } from '@angular/core';
import { Patient } from '../../models/patient';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  http=inject(HttpClient)
  
  getAllRoles():Observable<Role[]>{
    return this.http.get<Role[]>(`${URL.API_BASE}/roles`)
  }

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
}
