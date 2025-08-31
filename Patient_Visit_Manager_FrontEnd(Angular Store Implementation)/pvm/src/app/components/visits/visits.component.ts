import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { NavigationComponent } from '../navigation/navigation.component';
import { Visit } from '../../models/visit';
import { Patient } from '../../models/patient';
import { Doctor } from '../../models/doctor';
import { VisitType } from '../../models/visittype';
import { VisitState } from '../../store/state/visit.state';
import { PatientState } from '../../store/state/patient.state';
import { DoctorState } from '../../store/state/doctor.state';
import { VisitTypeState } from '../../store/state/visit-type.state';
import { AuthState } from '../../store/state/auth.state';
import { LoadVisits, CreateVisit, UpdateVisit, DeleteVisit } from '../../store/actions/visit.actions';
import { LoadPatients } from '../../store/actions/patient.actions';
import { LoadDoctors } from '../../store/actions/doctor.actions';
import { LoadVisitTypes } from '../../store/actions/visit-type.actions';

@Component({
  selector: 'app-visits',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NavigationComponent],
  templateUrl: './visits.component.html',
  styleUrls: ['./visits.component.scss']
})
export class VisitsComponent implements OnInit {
  store = inject(Store);
  fb = inject(FormBuilder);

  visits$ = this.store.select(VisitState.visits);
  patients$ = this.store.select(PatientState.patients);
  doctors$ = this.store.select(DoctorState.doctors);
  visitTypes$ = this.store.select(VisitTypeState.visitTypes);
  loading$ = this.store.select(VisitState.loading);
  userId$ = this.store.select(AuthState.userId);

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
    this.store.dispatch(new LoadPatients());
    this.store.dispatch(new LoadDoctors());
    this.store.dispatch(new LoadVisitTypes());
    this.store.dispatch(new LoadVisits());
  }

  loadVisits(): void {
    this.store.dispatch(new LoadVisits());
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
      this.store.dispatch(new DeleteVisit(id));
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

    this.userId$.subscribe(userId => {
      visitData.createdBy = userId || 1;
    }).unsubscribe();

    if (this.isEditing) {
      this.store.dispatch(new UpdateVisit(visitData)).subscribe({
        next: () => this.resetForm(),
        error: (error) => {
          console.error(error);
          alert(error.error || 'Save failed');
        }
      });
    } else {
      visitData.visitID = 0;
      this.store.dispatch(new CreateVisit(visitData)).subscribe({
        next: () => this.resetForm(),
        error: (error) => {
          console.error(error);
          alert(error.error || 'Save failed');
        }
      });
    }
  }

  resetForm(): void {
    this.userId$.subscribe(userId => {
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
        createdBy: userId || 1
      });
    }).unsubscribe();
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