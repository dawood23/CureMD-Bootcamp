import { Component, OnInit, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ApiService } from '../../../services/api/api.service';
import { Router } from '@angular/router';
import { AppointmentRequest } from '../../../models/appointmentRequest';
import { AuthService } from '../../../services/auth/auth.service';
import { Patient } from '../../../models/patient';
import { doctor } from '../../../models/doctor';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-appointment',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './create-appointment.component.html',
  styleUrl: './create-appointment.component.scss'
})
export class CreateAppointmentComponent implements OnInit {
  private fb = inject(FormBuilder);
  private api = inject(ApiService);
  private router = inject(Router);
  private authService = inject(AuthService);

  patients: Patient[] = [];
  doctors: doctor[] = [];
  userId: number = 0;

  errorMessage: string | null = null;
  successMessage: string | null = null;

  form = this.fb.group({
    patientID: [null, Validators.required],
    doctorID: [null, Validators.required],
    startTime: ['', Validators.required],
    durationMinutes: [30, [Validators.required, Validators.min(1)]],
    status: ['Scheduled', Validators.required]
  });

  ngOnInit(): void {
    this.userId = this.authService.getUserId() ?? 1;

    this.api.getAllPatients().subscribe({
      next: (res) => (this.patients = res),
      error: () => (this.errorMessage = 'Failed to load patients.')
    });

    this.api.getDoctors().subscribe({
      next: (res) => {(this.doctors = res)
        console.log(res)
        console.log(this.doctors)
      },
      
      error: () => (this.errorMessage = 'Failed to load doctors.')
    });
  }

  submit() {
    if (this.form.invalid) {
      this.errorMessage = 'Please fill out all required fields.';
      return;
    }

    const request: AppointmentRequest = {
      appointmentID: 0,
      patientID: this.form.value.patientID!,
      doctorID: this.form.value.doctorID!,
      createdBy: this.userId,
      startTime: this.form.value.startTime!,
      durationMinutes: this.form.value.durationMinutes!,
      status: this.form.value.status!
    };

    this.api.AddAppointment(request).subscribe({
      next: () => {
        this.successMessage = 'Appointment created successfully!';
        this.errorMessage = null;
        setTimeout(() => this.router.navigate(['/appointment-list']), 500);
      },
      error: (err) => {
        this.successMessage = null;
        this.errorMessage =
          err.error?.message || 'An error occurred while creating the appointment.';
      }
    });
  }

  cancel() {
    this.router.navigate(['/appointment-list']);
  }
}