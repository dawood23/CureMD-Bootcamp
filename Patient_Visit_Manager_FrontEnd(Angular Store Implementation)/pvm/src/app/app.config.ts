import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { importProvidersFrom } from '@angular/core';
import { authInterceptor } from './interceptors/auth/auth.interceptor';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { NgxsModule } from '@ngxs/store';
import { AuthState } from './store/state/auth.state';
import { PatientState } from './store/state/patient.state';
import { DoctorState } from './store/state/doctor.state';
import { UserRoleState } from './store/state/user-role.state';
import { UserState } from './store/state/user.state';
import { VisitState } from './store/state/visit.state';
import { VisitTypeState } from './store/state/visit-type.state';
import { NgxsLoggerPluginModule } from '@ngxs/logger-plugin';
import { NgxsReduxDevtoolsPluginModule } from '@ngxs/devtools-plugin';

export const appConfig: ApplicationConfig = {
   providers: [
    provideRouter(routes),
    provideHttpClient(withFetch(),withInterceptors([authInterceptor])),
    importProvidersFrom(FormsModule, ReactiveFormsModule),
    provideClientHydration(),
    importProvidersFrom(
      NgxsModule.forRoot([
        AuthState,
        PatientState,
        DoctorState,
        VisitState,
        VisitTypeState,
        UserRoleState,
        UserState
      ]),
      NgxsLoggerPluginModule.forRoot({
        disabled: false 
      }),
      NgxsReduxDevtoolsPluginModule.forRoot({
        disabled: false 
      })
    )
  ]
};
