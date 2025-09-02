
// visits.component.ts
import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { NavigationComponent } from '../navigation/navigation.component';
import { ApiService } from '../../services/api/api.service';
import { AuthService } from '../../services/auth/auth.service';
import { Visit } from '../../models/visit';
import { Patient } from '../../models/patient';
import { Doctor } from '../../models/doctor';
import { VisitType } from '../../models/visittype';

@Component({
  selector: 'app-visits',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NavigationComponent],
  templateUrl: './visits.component.html',
  styleUrls: ['./visits.component.scss']
})
export class VisitsComponent implements OnInit {
  api = inject(ApiService);
  auth = inject(AuthService);
  fb = inject(FormBuilder);

  visits: Visit[] = [];
  patients: Patient[] = [];
  doctors: Doctor[] = [];
  visitTypes: VisitType[] = [];
  isEditing = false;

  form = this.fb.group({
    visitID: this.fb.control<number>(0),
    patientID: this.fb.nonNullable.control(0, [Validators.required, Validators.min(1)]),
    doctorID: this.fb.control<number | null>(null),
    visitTypeID: this.fb.nonNullable.control(0, [Validators.required, Validators.min(1)]),
    visitDate: this.fb.nonNullable.control('', [Validators.required]),
    visitTime: this.fb.nonNullable.control('', [Validators.required]),
    description: this.fb.nonNullable.control(''),
    notes: this.fb.nonNullable.control(''),
    status: this.fb.nonNullable.control('Scheduled'),
    fee: this.fb.control<number | null>(null),
    createdBy: this.fb.control<number>(1)
  });

  get f() {
    return this.form.controls;
  }

  ngOnInit(): void {
    this.loadLookups();
    this.loadVisits();
  }

  loadLookups(): void {
    this.api.getPatients().subscribe({
      next: (patients) => this.patients = patients,
      error: () => console.error('Error loading patients')
    });

    this.api.getDoctors().subscribe({
      next: (doctors) => this.doctors = doctors,
      error: () => console.error('Error loading doctors')
    });

    this.api.getVisitTypes().subscribe({
      next: (visitTypes) => this.visitTypes = visitTypes,
      error: () => console.error('Error loading visit types')
    });
  }

  loadVisits(): void {
    this.api.getVisits().subscribe({
      next: (visits) => this.visits = visits,
      error: () => alert('Error loading visits.')
    });
  }

  editVisit(visit: Visit): void {
    this.form.patchValue({
      ...visit,
      visitDate: this.formatDate(visit.visitDate),
      visitTime: this.formatTime(visit.visitTime)
    });
    this.isEditing = true;
  }

  deleteVisit(id: number): void {
    if (confirm('Delete this visit?')) {
      this.api.deleteVisit(id).subscribe({
        next: () => this.loadVisits(),
        error: () => alert('Delete failed')
      });
    }
  }

  onSave(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const formValue = this.form.value;
    const visitData = { ...formValue } as Visit;
    
    if (visitData.visitTime && visitData.visitTime.length === 5) {
      visitData.visitTime = visitData.visitTime + ':00';
    }
    
    visitData.createdBy = this.auth.getUserId() || 1;

    if (this.isEditing) {
      this.api.updateVisit(visitData).subscribe({
        next: () => {
          this.loadVisits();
          this.resetForm();
        },
        error: (error) => {
          console.error(error);
          alert(error.error || 'Save failed');
        }
      });
    } else {
      visitData.visitID = 0;
      this.api.createVisit(visitData).subscribe({
        next: () => {
          this.loadVisits();
          this.resetForm();
        },
        error: (error) => {
          console.error(error);
          alert(error.error || 'Save failed');
        }
      });
    }
  }

  resetForm(): void {
    this.form.reset({
      visitID: 0,
      patientID: 0,
      doctorID: null,
      visitTypeID: 0,
      visitDate: '',
      visitTime: '',
      description: '',
      notes: '',
      status: 'Scheduled',
      fee: null,
      createdBy: this.auth.getUserId() || 1
    });
    this.isEditing = false;
  }

  formatDate(dateString?: string): string {
    if (!dateString) return '';
    return dateString.substring(0, 10);
  }

  formatTime(timeString?: string): string {
    if (!timeString) return '';
    return timeString.substring(0, 5);
  }
}