import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../../models/user';
import { Doctor } from '../../models/doctor';
import { Patient } from '../../models/patient';
import { Visit } from '../../models/visit';
import { VisitType } from '../../models/visittype';
import { UserRole } from '../../models/user-roles';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly API_BASE = 'http://localhost:5243/api';

  constructor(private http: HttpClient, private auth: AuthService) {}

  // Users
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.API_BASE}/Users`);
  }

  createUser(user: User): Observable<User> {
    const performedBy = this.auth.getUserId() || 1;
    return this.http.post<User>(`${this.API_BASE}/Users/${performedBy}`, user);
  }

  updateUser(id: number, user: User): Observable<User> {
    const performedBy = this.auth.getUserId() || 1;
    return this.http.put<User>(`${this.API_BASE}/Users/${id}/${performedBy}`, user);
  }

  deleteUser(id: number): Observable<any> {
    const performedBy = this.auth.getUserId() || 1;
    return this.http.delete(`${this.API_BASE}/Users/${id}/${performedBy}`);
  }

  // Doctors
  getDoctors(): Observable<Doctor[]> {
    return this.http.get<Doctor[]>(`${this.API_BASE}/Doctors`);
  }

  createDoctor(doctor: Doctor): Observable<Doctor> {
    const performedBy = this.auth.getUserId() || 1;
    return this.http.post<Doctor>(`${this.API_BASE}/Doctors/${performedBy}`, doctor);
  }

  updateDoctor(doctor: Doctor): Observable<Doctor> {
    const performedBy = this.auth.getUserId() || 1;
    return this.http.put<Doctor>(`${this.API_BASE}/Doctors/${performedBy}`, doctor);
  }

  deleteDoctor(id: number): Observable<any> {
    const performedBy = this.auth.getUserId() || 1;
    return this.http.delete(`${this.API_BASE}/Doctors/${id}/${performedBy}`);
  }

  // Patients
  getPatients(): Observable<Patient[]> {
    return this.http.get<Patient[]>(`${this.API_BASE}/Patients`);
  }

  createPatient(patient: Patient): Observable<Patient> {
    const performedBy = this.auth.getUserId() || 1;
    return this.http.post<Patient>(`${this.API_BASE}/Patients/${performedBy}`, patient);
  }

  updatePatient(patient: Patient): Observable<Patient> {
    const performedBy = this.auth.getUserId() || 1;
    return this.http.put<Patient>(`${this.API_BASE}/Patients/${performedBy}`, patient);
  }

  deletePatient(id: number): Observable<any> {
    const performedBy = this.auth.getUserId() || 1;
    return this.http.delete(`${this.API_BASE}/Patients/${id}/${performedBy}`);
  }

  // Visits
  getVisits(): Observable<Visit[]> {
    return this.http.get<Visit[]>(`${this.API_BASE}/Visits`);
  }

  createVisit(visit: Visit): Observable<Visit> {
    const performedBy = this.auth.getUserId() || 1;
    return this.http.post<Visit>(`${this.API_BASE}/Visits/${performedBy}`, visit);
  }

  updateVisit(visit: Visit): Observable<Visit> {
    const performedBy = this.auth.getUserId() || 1;
    return this.http.put<Visit>(`${this.API_BASE}/Visits/${performedBy}`, visit);
  }

  deleteVisit(id: number): Observable<any> {
    const performedBy = this.auth.getUserId() || 1;
    return this.http.delete(`${this.API_BASE}/Visits/${id}/${performedBy}`);
  }

  // Visit Types
  getVisitTypes(): Observable<VisitType[]> {
    return this.http.get<VisitType[]>(`${this.API_BASE}/VisitTypes`);
  }

  createVisitType(visitType: VisitType): Observable<VisitType> {
    return this.http.post<VisitType>(`${this.API_BASE}/VisitTypes/1`, visitType);
  }

  updateVisitType(visitType: VisitType): Observable<VisitType> {
    return this.http.put<VisitType>(`${this.API_BASE}/VisitTypes/1`, visitType);
  }

  deleteVisitType(id: number): Observable<any> {
    return this.http.delete(`${this.API_BASE}/VisitTypes/${id}/1`);
  }

  // User Roles
  getUserRoles(): Observable<UserRole[]> {
    return this.http.get<UserRole[]>(`${this.API_BASE}/UserRoles`);
  }

  createUserRole(role: UserRole): Observable<UserRole> {
    const performedBy = this.auth.getUserId() || 1;
    return this.http.post<UserRole>(`${this.API_BASE}/UserRoles/${performedBy}`, role);
  }

  updateUserRole(role: UserRole): Observable<UserRole> {
    const performedBy = this.auth.getUserId() || 1;
    return this.http.put<UserRole>(`${this.API_BASE}/UserRoles/${performedBy}`, role);
  }

  deleteUserRole(id: number): Observable<any> {
    const performedBy = this.auth.getUserId() || 1;
    return this.http.delete(`${this.API_BASE}/UserRoles/${id}/${performedBy}`);
  }
}