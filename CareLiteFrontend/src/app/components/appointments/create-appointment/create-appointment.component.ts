import { Component, OnInit, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AppointmentRequest } from '../../../models/appointmentRequest';
import { Patient } from '../../../models/patient';
import { doctor } from '../../../models/doctor';

import { Store, Select } from '@ngxs/store';
import { AddAppointment } from '../../../store/appointments/appointment.actions';
import { AppointmentState } from '../../../store/appointments/appointment.state';
import { PatientState } from '../../../store/patients/patient.state';
import { DoctorState } from '../../../store/doctor/doctor.state';
import { Observable } from 'rxjs';

import { AuthService } from '../../../services/auth/auth.service';
import { LoadPatients } from '../../../store/patients/patient.actions';
import { LoadDoctors } from '../../../store/doctor/doctor.actions';
import { appointmentValidator} from '../../../validator/time.validator';

@Component({
  selector: 'app-create-appointment',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './create-appointment.component.html',
  styleUrl: './create-appointment.component.scss'
})
export class CreateAppointmentComponent implements OnInit {
  private fb = inject(FormBuilder);
  private store = inject(Store);
  private router = inject(Router);
  private authService = inject(AuthService);
  public minDate:string=""

  @Select(PatientState.patients) patients$!: Observable<Patient[]>;
  @Select(DoctorState.doctors) doctors$!: Observable<doctor[]>;

  patients: Patient[] = [];
  doctors: doctor[] = [];
  userId: number = 0;

  errorMessage: string | null = null;
  successMessage: string | null = null;

  form = this.fb.group({
    patientID: [null, [Validators.required,Validators.nullValidator]],
    doctorID: [null, [Validators.required,Validators.nullValidator]],
    startTime: ['', [Validators.required,appointmentValidator]],
    durationMinutes: [30, [Validators.required, Validators.min(1)]],
    status: ['Scheduled', Validators.required]
  });

  ngOnInit(): void {
    this.userId = this.authService.getUserId() ?? 1;
    this.store.dispatch(new LoadPatients())
    this.store.dispatch(new LoadDoctors())
    this.patients$.subscribe(res => (this.patients = res));
    this.doctors$.subscribe(res => (this.doctors = res));

    this.minDate=new Date().toISOString().slice(0,16)
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

    this.store.dispatch(new AddAppointment(request)).subscribe({
      next: () => {
        this.successMessage = 'Appointment created successfully!';
        this.errorMessage = null;
        setTimeout(() => this.router.navigate(['/appointment-list']), 1500);
      },
      error: (err) => {
      this.successMessage = null;

      if (err.error?.errors) {
        const validationErrors = Object.values(err.error.errors)
          .flat()
          .join(' ');
        this.errorMessage = validationErrors;
      } else if (err.error?.message) {
        this.errorMessage = err.error.message;
      } else {
        this.errorMessage = 'An error occurred while creating the appointment.';
      }
    }

    });
  }

  cancel() {
    this.router.navigate(['/appointment-list']);
  }

}
