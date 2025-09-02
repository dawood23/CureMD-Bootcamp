import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { NavigationComponent } from '../navigation/navigation.component';
import { ApiService } from '../../services/api/api.service';
import { Patient } from '../../models/patient';
import { LetterOnlyDirective } from '../../directive/letter-only.directive';

@Component({
  selector: 'app-patients',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NavigationComponent,LetterOnlyDirective],
  templateUrl: './patients.component.html',
  styleUrls: ['./patients.component.scss']
})
export class PatientsComponent {
  api = inject(ApiService);
  fb = inject(FormBuilder);

  patients: Patient[] = [];
  isEditing = false;

  form = this.fb.group({
    patientID: this.fb.control<number>(0),
    firstName: this.fb.nonNullable.control('', [Validators.required]),
    lastName: this.fb.nonNullable.control('', [Validators.required]),
    dateOfBirth: this.fb.nonNullable.control('',[Validators.required]),
    phoneNumber: this.fb.nonNullable.control('',[Validators.required,Validators.maxLength(11),Validators.min(11)]),
    email: this.fb.nonNullable.control('', [Validators.email])
  });

  get f() {
    return this.form.controls;
  }

  ngOnInit(): void {
    this.loadPatients();
  }

  loadPatients(): void {
    this.api.getPatients().subscribe({
      next: (patients) => (this.patients = patients),
      error: () => alert('Error loading patients.')
    });
  }

  editPatient(patient: Patient): void {
    this.form.patchValue(patient);
    this.isEditing = true;
  }

  deletePatient(id: number): void {
    if (confirm('Delete this patient?')) {
      this.api.deletePatient(id).subscribe({
        next: () => this.loadPatients(),
        error: () => alert('Delete failed')
      });
    }
  }

  onSave(): void {
    if (this.form.invalid) {
      alert('First & Last name required');
      return;
    }

    const formValue = this.form.value as Patient;

    if (this.isEditing) {
      this.api.updatePatient(formValue).subscribe({
        next: () => {
          this.loadPatients();
          this.resetForm();
        },
        error: () => alert('Save failed')
      });
    } else {
      this.api.createPatient(formValue).subscribe({
        next: () => {
          this.loadPatients();
          this.resetForm();
        },
        error: () => alert('Save failed')
      });
    }
  }

  resetForm(): void {
    this.form.reset();
    this.isEditing = false;
  }
}
