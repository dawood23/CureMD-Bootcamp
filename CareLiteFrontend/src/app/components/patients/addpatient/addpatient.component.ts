import { Component } from '@angular/core';
import { Patient } from '../../../models/patient';
import { FormBuilder, ReactiveFormsModule,Validators} from '@angular/forms';
import { inject } from '@angular/core';
import { ApiService } from '../../../services/api/api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-addpatient',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './addpatient.component.html',
  styleUrl: './addpatient.component.scss'
})
export class AddpatientComponent {
  fb = inject(FormBuilder);
  patientService = inject(ApiService);
  router = inject(Router);

  form = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    dob: [''],
    gender: [''],
    phone: ['', Validators.required],
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

    this.patientService.createPatient(patient).subscribe({
      next: (res) => {
        alert(res.message || 'Patient created successfully');
        this.router.navigate(['/patient-list']); 
      },
      error: (err) => {
        alert(err.error?.message || 'Error creating patient');
      }
    });
  }

  goBack(){
    this.router.navigate(['/patient-list'])
  }
}
