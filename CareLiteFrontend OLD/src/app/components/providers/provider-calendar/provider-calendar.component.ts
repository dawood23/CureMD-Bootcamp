import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { addDays, startOfWeek, format } from 'date-fns';
import { ApiService } from '../../../services/api/api.service';
import { Appointment } from '../../../models/appointment';
import { doctor } from '../../../models/doctor';

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
  private api = inject(ApiService);

  doctors: doctor[] = [];
  appointments: Appointment[] = [];
  selectedDoctorId: number | null = null;

  currentWeekStart = startOfWeek(new Date(), { weekStartsOn: 1 });
  daysOfWeek: Date[] = [];
  timeSlots: string[] = [];
  slotDuration = 30;

  ngOnInit(): void {
    this.generateTimeSlots();
    this.generateWeekDays();
    this.api.getDoctors().subscribe(d => {
      this.doctors = d;
    });
  }

  generateTimeSlots() {
    this.timeSlots = [];
    const startHour = 8;
    const endHour = 18;
    const slotsPerHour = 60 / this.slotDuration;

    for (let h = startHour; h < endHour; h++) {
      for (let s = 0; s < slotsPerHour; s++) {
        const minutes = s * this.slotDuration;
        const timeString = `${h.toString().padStart(2, '0')}:${minutes
          .toString()
          .padStart(2, '0')}`;
        this.timeSlots.push(timeString);
      }
    }
  }

  generateWeekDays() {
    this.daysOfWeek = Array.from({ length: 7 }, (_, i) =>
      addDays(this.currentWeekStart, i)
    );
  }

  onSlotDurationChange() {
    this.generateTimeSlots();
    this.loadAppointments();
  }

  loadAppointments() {
    if (!this.selectedDoctorId) return;

    this.api.getAppointments().subscribe(allAppts => {
      this.appointments = allAppts.filter(
        a => a.doctorID === this.selectedDoctorId && a.status === 'Scheduled'
      );
    });
  }

  getSlotInfo(day: Date, slot: string): SlotInfo | null {
    const slotTime = this.parseSlotTime(day, slot);

    for (const appointment of this.appointments) {
      const apptStart = new Date(appointment.startTime);
      const apptEnd = new Date(
        apptStart.getTime() + appointment.durationMinutes * 60000
      );

      if (apptStart.toDateString() === day.toDateString()) {
        const slotEnd = new Date(slotTime.getTime() + this.slotDuration * 60000);

        if (slotTime < apptEnd && slotEnd > apptStart) {
          const isStart = slotTime.getTime() === apptStart.getTime();
          const isContinuation = !isStart;
          const durationInSlots = Math.ceil(
            appointment.durationMinutes / this.slotDuration
          );
          return { isStart, isContinuation, appointment, durationInSlots };
        }
      }
    }
    return null;
  }

  getSlotClass(day: Date, slot: string): string {
    const slotInfo = this.getSlotInfo(day, slot);
    if (!slotInfo) return 'free-slot';
    if (slotInfo.isStart) return 'appointment-start-slot';
    if (slotInfo.isContinuation) return 'appointment-cont-slot';
    return 'free-slot';
  }

  private parseSlotTime(day: Date, slot: string): Date {
    const [hours, minutes] = slot.split(':').map(Number);
    const slotDate = new Date(day);
    slotDate.setHours(hours, minutes, 0, 0);
    return slotDate;
  }

  formatTime(dateString: string): string {
    const date = new Date(dateString);
    return format(date, 'h:mm a');
  }

  formatEndTime(appointment: Appointment): string {
    const startTime = new Date(appointment.startTime);
    const endTime = new Date(
      startTime.getTime() + appointment.durationMinutes * 60000
    );
    return format(endTime, 'h:mm a');
  }

  addDays(date: Date, days: number): Date {
    return addDays(date, days);
  }

  prevWeek() {
    this.currentWeekStart = addDays(this.currentWeekStart, -7);
    this.generateWeekDays();
    this.loadAppointments();
  }

  nextWeek() {
    this.currentWeekStart = addDays(this.currentWeekStart, 7);
    this.generateWeekDays();
    this.loadAppointments();
  }

  // Helpers for trackBy
  trackByDate(index: number, day: Date): number {
    return day.getTime();
  }

  trackBySlot(index: number, slot: string): string {
    return slot;
  }
}
