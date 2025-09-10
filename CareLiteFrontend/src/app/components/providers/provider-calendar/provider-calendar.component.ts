import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Appointment } from '../../../models/appointment';
import { doctor } from '../../../models/doctor';
import { takeUntil } from 'rxjs';
import { WeeklyCalendar,CalendarDay,CalendarSlot } from '../../../models/calendar-models';
import { LoadDoctors } from '../../../store/doctor/doctor.actions';
import { CalendarState } from '../../../store/calendar/calendar.state';
import { DoctorState } from '../../../store/doctor/doctor.state';
import { LoadWeeklyCalendar,SetSelectedDoctor,SetSelectedWeek,SetSlotDuration } from '../../../store/calendar/calendar.actions';
import { Observable,Subject } from 'rxjs';
import { Store } from '@ngxs/store';


interface SlotInfo {
  isStart: boolean;
  isContinuation: boolean;
  appointment: Appointment;
  durationInSlots: number;
}

@Component({
  selector: 'app-provider-calendar',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './provider-calendar.component.html',
  styleUrls: ['./provider-calendar.component.scss']
})
export class ProviderCalendarComponent implements OnInit {
 private destroy$ = new Subject<void>();
  
  weeklyCalendar$: Observable<WeeklyCalendar | null>;
  doctors$: Observable<doctor[]>;
  selectedDoctor$: Observable<doctor | null>;
  selectedWeekStart$: Observable<Date>;
  slotDuration$: Observable<number>;
  loading$: Observable<boolean>;
  
  selectedDoctor: doctor | null = null;
  selectedWeekStart: Date = new Date();
  slotDuration: number = 15;
  
  slotDurations = [
    { value: 15, label: '15 minutes' },
    { value: 30, label: '30 minutes' },
    { value: 60, label: '60 minutes' }
  ];

  constructor(private store: Store) {
    this.weeklyCalendar$ = this.store.select(CalendarState.weeklyCalendar);
    this.doctors$ = this.store.select(DoctorState.doctors);
    this.selectedDoctor$ = this.store.select(CalendarState.selectedDoctor);
    this.selectedWeekStart$ = this.store.select(CalendarState.selectedWeekStart);
    this.slotDuration$ = this.store.select(CalendarState.slotDuration);
    this.loading$ = this.store.select(CalendarState.loading);
  }

  ngOnInit(): void {

    this.store.dispatch(new LoadDoctors());
    
    this.selectedDoctor$.pipe(takeUntil(this.destroy$)).subscribe(doctor => {
      this.selectedDoctor = doctor;
    });
    
    this.selectedWeekStart$.pipe(takeUntil(this.destroy$)).subscribe(date => {
      this.selectedWeekStart = date;
    });
    
    this.slotDuration$.pipe(takeUntil(this.destroy$)).subscribe(duration => {
      this.slotDuration = duration;
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }


  onSlotDurationChange(duration: number): void {
    this.store.dispatch(new SetSlotDuration(duration));
  }

  navigateWeek(direction: 'prev' | 'next'): void {
    const newDate = new Date(this.selectedWeekStart);
    const daysToAdd = direction === 'next' ? 7 : -7;
    newDate.setDate(newDate.getDate() + daysToAdd);
    this.store.dispatch(new SetSelectedWeek(newDate));
  }

  goToCurrentWeek(): void {
    const today = new Date();
    const startOfWeek = this.getStartOfWeek(today);
    this.store.dispatch(new SetSelectedWeek(startOfWeek));
  }

  private getStartOfWeek(date: Date): Date {
    const day = date.getDay();
    const diff = date.getDate() - day + (day === 0 ? -6 : 1); 
    const startOfWeek = new Date(date);
    startOfWeek.setDate(diff);
    startOfWeek.setHours(0, 0, 0, 0);
    return startOfWeek;
  }

  getWeekDateRange(calendar: WeeklyCalendar): string {
    const start = calendar.weekStartDate.toLocaleDateString('en-US', { 
      month: 'short', 
      day: 'numeric' 
    });
    const end = calendar.weekEndDate.toLocaleDateString('en-US', { 
      month: 'short', 
      day: 'numeric',
      year: 'numeric'
    });
    return `${start} - ${end}`;
  }

  getSlotClass(slot: CalendarSlot): string {
    const baseClass = 'calendar-slot';
    if (slot.isBooked) {
      return `${baseClass} booked`;
    }
    return `${baseClass} available`;
  }

  getAppointmentDurationClass(appointment: any): string {
    if (!appointment) return '';
    return `duration-${appointment.durationMinutes}`;
  }

  formatTime(time: string): string {
    const [hours, minutes] = time.split(':');
    const hour = parseInt(hours);
    const ampm = hour >= 12 ? 'PM' : 'AM';
    const displayHour = hour > 12 ? hour - 12 : (hour === 0 ? 12 : hour);
    return `${displayHour}:${minutes} ${ampm}`;
  }

  onSlotClick(day: CalendarDay, slot: CalendarSlot): void {
    if (!slot.isBooked && this.selectedDoctor) {
      console.log('Selected slot:', day.date, slot.time, this.selectedDoctor);
    }
  }

  getTimeSlots(slots: CalendarSlot[]): string[] {
    return slots ? slots.map(slot => slot.time) : [];
  }


  onDoctorChange(event: any): void {
    const doctorId = parseInt(event.target.value);
    if (doctorId) {
      const doctors = this.store.selectSnapshot(DoctorState.doctors);
      const doctor = doctors.find(d => d.doctorID === doctorId);
      if (doctor) {
        this.store.dispatch(new SetSelectedDoctor(doctor));
      }
    } else {
      this.store.dispatch(new SetSelectedDoctor(null));
    }
  }
}
