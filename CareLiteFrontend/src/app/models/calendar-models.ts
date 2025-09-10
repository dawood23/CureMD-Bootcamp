export interface CalendarSlot {
  time: string;
  duration: number; 
  isBooked: boolean;
  appointment?: CalendarAppointment;
  slotType: 'available' | 'booked' | 'partial'; 
}

export interface CalendarAppointment {
  appointmentID: number;
  startTime: string;
  durationMinutes: number;
  status: string;
  patientID: number;
  patientName: string;
  doctorID: number;
  doctorName: string;
  appointmentDate: string;
  appointmentTime: string;
  endTime: string;
}

export interface CalendarDay {
  date: Date;
  dayName: string;
  slots: CalendarSlot[];
  isToday: boolean;
}

export interface WeeklyCalendar {
  weekStartDate: Date;
  weekEndDate: Date;
  days: CalendarDay[];
  selectedDoctor?: any;
}