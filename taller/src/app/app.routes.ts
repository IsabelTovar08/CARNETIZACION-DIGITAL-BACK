import { Routes } from '@angular/router';

export const routes: Routes = [
{
    path: 'auth',
    loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'appointments',
    loadChildren: () =>
      import('./gestion-appointments/gestion-appointments.module').then(m => m.GestionAppointmentsModule)
  },
{ path: '', redirectTo: 'auth/login', pathMatch: 'full' }

];
