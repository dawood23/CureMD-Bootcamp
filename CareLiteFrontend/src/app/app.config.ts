import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './interceptor/auth.interceptor';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { PatientState } from './store/patients/patient.state';
import { DoctorState } from './store/doctor/doctor.state';
import { RoleState } from './store/roles/role.state';
import { AppointmentState } from './store/appointments/appointment.state';
import { CalendarState } from './store/calendar/calendar.state';
import { VisitState } from './store/visitNotes/visitNotes.state';
import { NgxsModule } from '@ngxs/store';
import { MatDialogModule } from '@angular/material/dialog';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns'; 

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(
      withFetch(),
      withInterceptors([authInterceptor])
    ),
    provideAnimationsAsync(),
    importProvidersFrom(
      NgxsModule.forRoot([
        RoleState,
        PatientState,
        DoctorState,
        AppointmentState,
        CalendarState,
        VisitState
      ]),
      MatDialogModule,
      CalendarModule.forRoot({ provide: DateAdapter, useFactory: adapterFactory }) 
    ), provideAnimationsAsync()
  ]
};
