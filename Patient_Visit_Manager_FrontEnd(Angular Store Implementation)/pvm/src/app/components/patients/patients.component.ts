import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { NavigationComponent } from '../navigation/navigation.component';
import { Patient } from '../../models/patient';
import { LetterOnlyDirective } from '../../directive/letter-only.directive';
import { PatientState } from '../../store/state/patient.state';
import { LoadPatients,CreatePatient,UpdatePatient,DeletePatient } from '../../store/actions/patient.actions';

@Component({
  selector: 'app-patients',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NavigationComponent, LetterOnlyDirective],
  templateUrl: './patients.component.html',
  styleUrls: ['./patients.component.scss']
})
export class PatientsComponent implements OnInit {
  store = inject(Store);
  fb = inject(FormBuilder);

  patients$ = this.store.select(PatientState.patients);
  loading$ = this.store.select(PatientState.loading);
  error$ = this.store.select(PatientState.error);
  
  isEditing = false;

  form = this.fb.group({
    patientID: this.fb.control<number>(0),
    firstName: this.fb.nonNullable.control('', [Validators.required]),
    lastName: this.fb.nonNullable.control('', [Validators.required]),
    dateOfBirth: this.fb.nonNullable.control('', [Validators.required]),
    phoneNumber: this.fb.nonNullable.control('', [Validators.required, Validators.maxLength(11), Validators.minLength(11)]),
    email: this.fb.nonNullable.control('', [Validators.email])
  });

  get f() {
    return this.form.controls;
  }

  ngOnInit(): void {
    this.store.dispatch(new LoadPatients());
  }

  editPatient(patient: Patient): void {
    this.form.patchValue(patient);
    this.isEditing = true;
  }

  deletePatient(id: number): void {
    if (confirm('Delete this patient?')) {
      this.store.dispatch(new DeletePatient(id));
    }
  }
  loadPatients(): void {
  this.store.dispatch(new LoadPatients());
}

  onSave(): void {
    if (this.form.invalid) {
      alert('First & Last name required');
      return;
    }

    const formValue = this.form.value as Patient;

    if (this.isEditing) {
      this.store.dispatch(new UpdatePatient(formValue)).subscribe({
        next: () => this.resetForm(),
        error: () => alert('Save failed')
      });
    } else {
      this.store.dispatch(new CreatePatient(formValue)).subscribe({
        next: () => this.resetForm(),
        error: () => alert('Save failed')
      });
    }
  }

  resetForm(): void {
    this.form.reset();
    this.isEditing = false;
  }
}