import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimations } from '@angular/platform-browser/animations';
import {provideHttpClient, withFetch, withInterceptors} from "@angular/common/http";
import { authInterceptor } from './common/auth/Auth.interceptor';


export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    // provideToastr({
    // }),
    provideAnimations(),
    provideHttpClient(withInterceptors([authInterceptor])),
  ]
};
