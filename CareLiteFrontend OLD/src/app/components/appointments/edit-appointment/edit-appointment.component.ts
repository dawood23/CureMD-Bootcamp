import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { ApiService } from '../../../services/api/api.service';
import { AppointmentRequest } from '../../../models/appointmentRequest';
import { Patient } from '../../../models/patient';
import { doctor } from '../../../models/doctor';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-appointment',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './edit-appointment.component.html',
  styleUrl: './edit-appointment.component.scss'
})
export class EditAppointmentComponent implements OnInit {
  appointmentId!: number;
  patients: Patient[] = [];
  doctors: doctor[] = [];
  errorMessage: string | null = null;
  successMessage: string | null = null;
  patientid:number=0
  patientName: string = '';

  form = this.fb.group({
    appointmentID: [0],
    patientID: [0],
    doctorID: [0, Validators.required],
    startTime: ['', Validators.required],
    durationMinutes: [30, [Validators.required, Validators.min(1)]],
    status: ['Scheduled', Validators.required]
  });

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private api: ApiService
  ) {}

  ngOnInit(): void {
    this.appointmentId = Number(this.route.snapshot.paramMap.get('id'));

  
    this.api.getDoctors().subscribe(d => (this.doctors = d));


    this.api.getAppointments().subscribe(appts => {
      const appt = appts.find(a => a.appointmentID === this.appointmentId);
      if (appt) {
        this.patientName = appt.patientName; 
        this.patientid=appt.patientID

        this.form.patchValue({
          appointmentID: appt.appointmentID,
          patientID: appt.patientID,
          doctorID: appt.doctorID,
          startTime: appt.startTime.substring(0, 16), 
          durationMinutes: appt.durationMinutes,
          status: appt.status
        });
      }
    });
  }

  submit() {
    if (this.form.invalid) {
      this.errorMessage = 'Please fill out all required fields.';
      return;
    }

    const request = this.form.value as AppointmentRequest;

    this.api.updateAppointment(request).subscribe({
      next: () => {
        this.successMessage = 'Appointment updated successfully!';
        this.errorMessage = null;
        setTimeout(() => this.router.navigate(['/appointment-list']), 500);
      },
      error: (err) => {
        this.successMessage = null;
        this.errorMessage = err.error?.message || 'Error updating appointment.';
      }
    });
  }

  cancel() {
    this.router.navigate(['/appointment-list']);
  }
}
