import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { importProvidersFrom } from '@angular/core';
import { authInterceptor } from './interceptors/auth/auth.interceptor';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';

export const appConfig: ApplicationConfig = {
   providers: [
    provideRouter(routes),
    provideHttpClient(withFetch(),withInterceptors([authInterceptor])),
    importProvidersFrom(FormsModule, ReactiveFormsModule),
    provideClientHydration()
  ]
};
