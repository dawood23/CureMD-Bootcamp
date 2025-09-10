import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Appointment } from '../../../models/appointment';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime } from 'rxjs';
import { Router } from '@angular/router';
import { HighlightPipe } from '../../../pipes/highlight/highlight.pipe';

// NGXS
import { Store, Select } from '@ngxs/store';
import { AppointmentState } from '../../../store/appointments/appointment.state';
import { LoadAppointments, DeleteAppointment } from '../../../store/appointments/appointment.actions';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-appointment-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HighlightPipe],
  templateUrl: './appointment-list.component.html',
  styleUrls: ['./appointment-list.component.scss']
})
export class AppointmentListComponent {
  store = inject(Store);
  router = inject(Router);

  @Select(AppointmentState.appointments) appointments$!: Observable<Appointment[]>;

  appointments: Appointment[] = [];
  filteredAppointments: Appointment[] = [];
  searchControl = new FormControl('');

  currentPage = 1;
  pageSize = 5;

  ngOnInit() {
    this.store.dispatch(new LoadAppointments());

    this.appointments$.subscribe(appointments => {
      this.appointments = appointments;
      this.filteredAppointments = appointments;
    });

    this.searchControl.valueChanges
      .pipe(debounceTime(300))
      .subscribe(query => {
        const lowerQuery = query?.toLowerCase() || '';
        this.filteredAppointments = this.appointments.filter(a =>
          a.patientName.toLowerCase().includes(lowerQuery) ||
          a.doctorName.toLowerCase().includes(lowerQuery) ||
          a.status.toLowerCase().includes(lowerQuery)
        );
        this.currentPage = 1;
      });
  }

  get paginatedAppointments(): Appointment[] {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    return this.filteredAppointments.slice(startIndex, startIndex + this.pageSize);
  }

  nextPage() {
    if (this.currentPage * this.pageSize < this.filteredAppointments.length) {
      this.currentPage++;
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  get totalPages(): number {
    return Math.ceil(this.filteredAppointments.length / this.pageSize);
  }

  redirect() {
    this.router.navigate(['/appointment-add']);
  }

  goBack() {
    this.router.navigate(['/dashboard']);
  }

  editAppointment(id: number) {
    this.router.navigate([`/appointments/edit/${id}`]);
  }

  deleteAppointment(id: number) {
    this.store.dispatch(new DeleteAppointment(id)).subscribe({
      next: () => {
        this.appointments = this.appointments.filter(a => a.appointmentID !== id);
        this.filteredAppointments = this.filteredAppointments.filter(a => a.appointmentID !== id);
      },
      error: (e) => console.log(e)
    });
  }
}
