import { State, Action, StateContext, Selector } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { ApiService } from '../../services/api/api.service';
import { CalendarAppointment, WeeklyCalendar, CalendarDay, CalendarSlot } from '../../models/calendar-models';
import { LoadWeeklyCalendar, SetSelectedDoctor, SetSelectedWeek, SetSlotDuration } from './calendar.actions';

export interface CalendarStateModel {
  weeklyCalendar: WeeklyCalendar | null;
  appointments: CalendarAppointment[];
  selectedDoctor: any | null;
  selectedWeekStart: Date;
  slotDuration: number; 
  loading: boolean;
}

@State<CalendarStateModel>({
  name: 'calendar',
  defaults: {
    weeklyCalendar: null,
    appointments: [],
    selectedDoctor: null,
    selectedWeekStart: CalendarState.getStartOfWeek(new Date()),
    slotDuration: 15,
    loading: false
  }
})
@Injectable()
export class CalendarState {
  constructor(private api: ApiService) {}

  @Selector()
  static weeklyCalendar(state: CalendarStateModel) {
    return state.weeklyCalendar;
  }

  @Selector()
  static selectedDoctor(state: CalendarStateModel) {
    return state.selectedDoctor;
  }

  @Selector()
  static selectedWeekStart(state: CalendarStateModel) {
    return state.selectedWeekStart;
  }

  @Selector()
  static slotDuration(state: CalendarStateModel) {
    return state.slotDuration;
  }

  @Selector()
  static loading(state: CalendarStateModel) {
    return state.loading;
  }

  @Action(LoadWeeklyCalendar)
  loadWeeklyCalendar({ patchState, getState }: StateContext<CalendarStateModel>, { doctorId, weekStartDate }: LoadWeeklyCalendar) {
    patchState({ loading: true });
    
    return this.api.getWeeklyCalendar(doctorId, weekStartDate).pipe(
      tap(appointments => {
        const state = getState();
        const calendar = this.buildWeeklyCalendar(appointments, new Date(weekStartDate), state.slotDuration, state.selectedDoctor);
        patchState({ 
          appointments,
          weeklyCalendar: calendar,
          loading: false 
        });
      })
    );
  }

  @Action(SetSelectedDoctor)
  setSelectedDoctor({ patchState, dispatch, getState }: StateContext<CalendarStateModel>, { doctor }: SetSelectedDoctor) {
    const state = getState();
    patchState({ selectedDoctor: doctor });
    
    if (doctor) {
      const weekStartStr = this.formatDate(state.selectedWeekStart);
      dispatch(new LoadWeeklyCalendar(doctor.doctorID, weekStartStr));
    }
  }

  @Action(SetSelectedWeek)
  setSelectedWeek({ patchState, dispatch, getState }: StateContext<CalendarStateModel>, { weekStartDate }: SetSelectedWeek) {
    const state = getState();
    patchState({ selectedWeekStart: weekStartDate });
    
    if (state.selectedDoctor) {
      const weekStartStr = this.formatDate(weekStartDate);
      dispatch(new LoadWeeklyCalendar(state.selectedDoctor.doctorID, weekStartStr));
    }
  }

  @Action(SetSlotDuration)
  setSlotDuration({ patchState, getState }: StateContext<CalendarStateModel>, { duration }: SetSlotDuration) {
    const state = getState();
    patchState({ slotDuration: duration });
    
    if (state.appointments.length > 0 && state.selectedDoctor) {
      const calendar = this.buildWeeklyCalendar(state.appointments, state.selectedWeekStart, duration, state.selectedDoctor);
      patchState({ weeklyCalendar: calendar });
    }
  }

  private static getStartOfWeek(date: Date): Date {
    const day = date.getDay();
    const diff = date.getDate() - day + (day === 0 ? -6 : 1); 
    return new Date(date.setDate(diff));
  }

  private formatDate(date: Date): string {
    return date.toISOString().split('T')[0];
  }

  private buildWeeklyCalendar(appointments: CalendarAppointment[], weekStart: Date, slotDuration: number, selectedDoctor: any): WeeklyCalendar {
    const days: CalendarDay[] = [];
    const today = new Date();
    
    for (let i = 0; i < 7; i++) {
      const currentDate = new Date(weekStart);
      currentDate.setDate(weekStart.getDate() + i);
      
      const dayAppointments = appointments.filter(apt => 
        new Date(apt.appointmentDate).toDateString() === currentDate.toDateString()
      );
      
      const slots = this.generateDaySlots(dayAppointments, slotDuration);
      
      days.push({
        date: new Date(currentDate),
        dayName: currentDate.toLocaleDateString('en-US', { weekday: 'short' }),
        slots,
        isToday: currentDate.toDateString() === today.toDateString()
      });
    }

    const weekEnd = new Date(weekStart);
    weekEnd.setDate(weekStart.getDate() + 6);

    return {
      weekStartDate: new Date(weekStart),
      weekEndDate: weekEnd,
      days,
      selectedDoctor
    };
  }

  private generateDaySlots(appointments: CalendarAppointment[], slotDuration: number): CalendarSlot[] {
    const slots: CalendarSlot[] = [];
    const businessStart = 9 * 60; 
    const businessEnd = 17 * 60; 
    
    for (let time = businessStart; time < businessEnd; time += slotDuration) {
      const hours = Math.floor(time / 60);
      const minutes = time % 60;
      const timeStr = `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`;
      
      const appointment = this.findAppointmentForSlot(appointments, timeStr, slotDuration);
      
      slots.push({
        time: timeStr,
        duration: slotDuration,
        isBooked: !!appointment,
        appointment,
        slotType: appointment ? 'booked' : 'available'
      });
    }
    
    return slots;
  }

  private findAppointmentForSlot(appointments: CalendarAppointment[], slotTime: string, slotDuration: number): CalendarAppointment | undefined {
    return appointments.find(apt => {
      const aptTime = new Date(`2000-01-01T${apt.appointmentTime}`);
      const aptEndTime = new Date(aptTime.getTime() + apt.durationMinutes * 60000);
      const slotStartTime = new Date(`2000-01-01T${slotTime}`);
      const slotEndTime = new Date(slotStartTime.getTime() + slotDuration * 60000);
      
      // Check if appointment overlaps with this slot
      return (aptTime < slotEndTime && aptEndTime > slotStartTime);
    });
  }
}