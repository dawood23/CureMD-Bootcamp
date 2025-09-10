import { Component, inject } from '@angular/core';
import { Patient } from '../../../models/patient';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LetterOnlyDirective } from '../../../directive/letter-only/letter-only.directive';
import { DigitOnlyDirective } from '../../../directive/digit-only/digit-only.directive';
import { Store } from '@ngxs/store';
import { AddPatient } from '../../../store/patients/patient.actions';

@Component({
  selector: 'app-addpatient',
  standalone: true,
  imports: [ReactiveFormsModule, LetterOnlyDirective, DigitOnlyDirective],
  templateUrl: './addpatient.component.html',
  styleUrl: './addpatient.component.scss'
})
export class AddpatientComponent {
  fb = inject(FormBuilder);
  store = inject(Store);
  router = inject(Router);

  maxdate = new Date().toISOString().split('T')[0];

  form = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    dob: [''],
    gender: [''],
    phone: ['', [Validators.required, Validators.minLength(11), Validators.maxLength(11)]],
    email: ['', [Validators.email]],
    address: ['']
  });

  formControl = this.form.controls;

  submit() {
    if (this.form.invalid) {
      alert('Please fill all required fields');
      return;
    }

    const patient: Patient = {
      patientID: 0,
      firstName: this.form.value.firstName!,
      lastName: this.form.value.lastName!,
      dob: this.form.value.dob ? new Date(this.form.value.dob) : null,
      gender: this.form.value.gender!,
      phone: this.form.value.phone!,
      email: this.form.value.email!,
      address: this.form.value.address!,
      createdAt: new Date()
    };

    this.store.dispatch(new AddPatient(patient)).subscribe({
      next: () => {
        alert('Patient created successfully');
        this.router.navigate(['/patient-list']);
      },
      error: (err) => {
        alert(err.error?.message || 'Error creating patient');
      }
    });
  }

  goBack() {
    this.router.navigate(['/patient-list']);
  }
}
