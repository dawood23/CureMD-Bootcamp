import { Component, OnInit, inject, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, Subject } from 'rxjs';
import { ApiService } from '../../../services/api/api.service';
import { CalendarAppointment } from '../../../models/calendar-models';

import { 
  CalendarEvent, 
  CalendarModule
} from 'angular-calendar';
import { addMinutes, startOfWeek, format, addDays } from 'date-fns';
import { Select, Store } from '@ngxs/store';
import { DoctorState } from '../../../store/doctor/doctor.state';
import { doctor } from '../../../models/doctor';
import { LoadDoctors } from '../../../store/doctor/doctor.actions';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-calendar',
  standalone: true,
  imports: [CommonModule, CalendarModule,RouterLink],
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CalendarComponent implements OnInit {
  private calendarService = inject(ApiService);
  private store = inject(Store);

  viewDate: Date = new Date();
  events: CalendarEvent[] = [];
  refresh$ = new Subject<void>();

  @Select(DoctorState.doctors) doctors$!: Observable<doctor[]>;

  doctorId: number | null = null; 
  selectedDoctor: doctor | null = null;

  dayStartHour: number = 9;
  dayEndHour: number = 17;
  hourSegments: number = 4;
  hourSegmentHeight: number = 30;
  weekStartsOn: 0 | 1 | 2 | 3 | 4 | 5 | 6 = 1;

  ngOnInit(): void {
    this.store.dispatch(new LoadDoctors());
  }

  onDoctorChange(event: any): void {
    const doctorId = parseInt(event.target.value, 10);
    if (doctorId) {
      const doctors = this.store.selectSnapshot(DoctorState.doctors);
      this.selectedDoctor = doctors.find(d => d.doctorID === doctorId) || null;
      this.doctorId = doctorId;
      this.loadCalendar();
    } else {
      this.selectedDoctor = null;
      this.doctorId = null;
      this.events = [];
    }
  }

  changeWeek(days: number): void {
    const newDate = new Date(this.viewDate);
    newDate.setDate(newDate.getDate() + days);
    this.viewDate = newDate;
    if (this.doctorId) {
      this.loadCalendar();
    }
  }

  private loadCalendar(): void {
    if (!this.doctorId) return;

    const weekStart = startOfWeek(this.viewDate, { weekStartsOn: this.weekStartsOn });
    const formattedDate = format(weekStart, 'yyyy-MM-dd');

    this.calendarService
      .getWeeklyCalendar(this.doctorId, formattedDate)
      .subscribe({
        next: (appointments: CalendarAppointment[]) => {
          this.events = appointments.map(a => this.mapToEvent(a));
          this.refresh$.next();
        },
        error: err => console.error('Failed to load calendar', err),
      });
  }

  private mapToEvent(appointment: CalendarAppointment): CalendarEvent {
    const appointmentDateTime = new Date(`${appointment.appointmentDate}T${appointment.appointmentTime}`);
    const start = new Date(appointment.startTime) || appointmentDateTime;
    const end = appointment.durationMinutes
      ? addMinutes(start, appointment.durationMinutes)
      : new Date(appointment.endTime);

    return {
      start,
      end,
      title: `${appointment.patientName}`,
      meta: {
        appointmentId: appointment.appointmentID,
        status: appointment.status,
        patientId: appointment.patientID,
        doctorId: appointment.doctorID
      },
      color: this.getEventColor(appointment.status),
      draggable: false,
      resizable: { beforeStart: false, afterEnd: false },
      cssClass: 'cal-appointment'
    };
  }

  private getEventColor(status: string): { primary: string; secondary: string } {
    switch (status.toLowerCase()) {
      case 'confirmed':
        return { primary: '#28a745', secondary: '#d4edda' };
      case 'pending':
        return { primary: '#ffc107', secondary: '#fff3cd' };
      case 'cancelled':
        return { primary: '#dc3545', secondary: '#f8d7da' };
      case 'completed':
        return { primary: '#6c757d', secondary: '#f8f9fa' };
      default:
        return { primary: '#007bff', secondary: '#cce5ff' };
    }
  }

  markSlot(date: Date): void {
    if (!this.selectedDoctor) return;

    const isOccupied = this.events.some(event => {
      const eventStart = new Date(event.start);
      const eventEnd = new Date(event.end!);
      return date >= eventStart && date < eventEnd;
    });

    if (isOccupied) {
      console.log('Slot already occupied');
      return;
    }

    const newSlot: CalendarEvent = {
      start: date,
      end: addMinutes(date, 15),
      title: 'Available',
      color: { primary: '#28a745', secondary: '#d4edda' },
      draggable: false,
      resizable: { beforeStart: false, afterEnd: false },
      cssClass: 'cal-available-slot',
      meta: {
        type: 'available-slot'
      }
    };

    this.events = [...this.events, newSlot];
    this.refresh$.next();
  }

  removeSlot(event: CalendarEvent): void {
    if (event.meta?.type === 'available-slot') {
      this.events = this.events.filter(e => e !== event);
      this.refresh$.next();
    }
  }

  onEventClicked(event: CalendarEvent): void {
    if (event.meta?.type === 'available-slot') {
      this.removeSlot(event);
    } else {
      console.log('Appointment clicked:', event.meta);
    }
  }

  getCurrentWeekRange(): string {
    const weekStart = startOfWeek(this.viewDate, { weekStartsOn: this.weekStartsOn });
    const weekEnd = addDays(weekStart, 6);
    return `${format(weekStart, 'MMM d')} - ${format(weekEnd, 'MMM d, yyyy')}`;
  }

  isCurrentWeek(): boolean {
    const today = new Date();
    const weekStart = startOfWeek(this.viewDate, { weekStartsOn: this.weekStartsOn });
    const weekEnd = addDays(weekStart, 6);
    return today >= weekStart && today <= weekEnd;
  }

  goToToday(): void {
    this.viewDate = new Date();
    if (this.doctorId) {
      this.loadCalendar();
    }
  }
}
