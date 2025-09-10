import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../services/api/api.service';
import { Appointment } from '../../../models/appointment';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, switchMap } from 'rxjs';
import { Router } from '@angular/router';
import { HighlightPipe } from '../../../pipes/highlight/highlight.pipe';

@Component({
  selector: 'app-appointment-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HighlightPipe],
  templateUrl: './appointment-list.component.html',
  styleUrls: ['./appointment-list.component.scss']
})
export class AppointmentListComponent {
  appointmentService = inject(ApiService);
  router = inject(Router);

  appointments: Appointment[] = [];
  filteredAppointments: Appointment[] = [];
  searchControl = new FormControl('');

  currentPage = 1;
  pageSize = 5;

  ngOnInit() {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(300),
        switchMap(() => this.appointmentService.getAppointments())
      )
      .subscribe(appointments => {
        const query = this.searchControl.value?.toLowerCase() || '';

        this.filteredAppointments = appointments.filter(a =>
          a.patientName.toLowerCase().includes(query) ||
          a.doctorName.toLowerCase().includes(query) ||
          a.status.toLowerCase().includes(query)
        );
        this.currentPage = 1;
      });

    this.appointmentService.getAppointments().subscribe({
      next: (appointments) => {
        this.appointments = appointments;
        this.filteredAppointments = appointments;
      },
      error: () => alert('An error occurred while loading the appointments')
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

  deleteAppointment(id:number){
    this.appointmentService.deleteAppointment(id).subscribe({
      next:()=>{
        this.appointments=this.appointments.filter(a=>a.appointmentID!=id)
        this.filteredAppointments=this.filteredAppointments.filter(a=>a.appointmentID!=id)  
      },
      error:(e)=>console.log(e)
    })
  }
}
