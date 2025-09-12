import { Component, ViewChild, AfterViewInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Appointment } from '../../../models/appointment';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime } from 'rxjs';
import { Router, RouterLink } from '@angular/router';
import { HighlightPipe } from '../../../pipes/highlight/highlight.pipe';

import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

import { Store, Select } from '@ngxs/store';
import { AppointmentState } from '../../../store/appointments/appointment.state';
import { LoadAppointments, DeleteAppointment } from '../../../store/appointments/appointment.actions';
import { Observable } from 'rxjs';
import { AuthService } from '../../../services/auth/auth.service';

@Component({
  selector: 'app-appointment-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    HighlightPipe,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
  templateUrl: './appointment-list.component.html',
  styleUrls: ['./appointment-list.component.scss']
})
export class AppointmentListComponent implements AfterViewInit {
  store = inject(Store);
  router = inject(Router);
  role:string=""
  authService=inject(AuthService)
  @Select(AppointmentState.appointments) appointments$!: Observable<Appointment[]>;

  displayedColumns: string[] = [
    'appointmentID',
    'patientName',
    'doctorName',
    'startTime',
    'durationMinutes',
    'status',
    'createdAt',
    'actions'
  ];
  dataSource = new MatTableDataSource<Appointment>();

  searchControl = new FormControl('');

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit() {
    this.store.dispatch(new LoadAppointments());
    
    this.role=this.authService.getUserRole()??""
    
    this.appointments$.subscribe(appointments => {
      this.dataSource.data = appointments;
    });

    this.searchControl.valueChanges
      .pipe(debounceTime(300))
      .subscribe(query => {
        this.dataSource.filter = query?.trim().toLowerCase() || '';
      });

    this.dataSource.filterPredicate = (data: Appointment, filter: string) => {
      const str = filter.toLowerCase();
      return (
        data.patientName.toLowerCase().includes(str) ||
        data.doctorName.toLowerCase().includes(str) ||
        data.status.toLowerCase().includes(str)
      );
    };
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
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
        this.dataSource.data = this.dataSource.data.filter(a => a.appointmentID !== id);
      },
      error: (e) => console.log(e)
    });
  }

  goToVisitNotes(id: number) {
    this.router.navigate(['/visit-note'], {
      queryParams: { appID: id }
    });
  }

  generateBill(id:number){
    this.router.navigate(['/generate-bill'],{
      queryParams:{appID:id}
    })
  }
}
