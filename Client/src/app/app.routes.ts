import { Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login-component/login-component';
import { RegisterComponent } from './components/auth/register-component/register-component';
import { VenuesComponent } from './components/venues-component/venues-component';
import { authGuard } from './guards/auth-guard';
import { VenueComponent } from './components/venue-component/venue-component';
import { BookingsComponent } from './components/bookings-component/bookings-component';
import { RoleRequestsComponent } from './components/role-requests-component/role-requests-component';

export const routes: Routes = [
  { path: 'venues', component: VenuesComponent, canActivate: [authGuard] },
  { path: 'venue/:slug', component: VenueComponent, canActivate: [authGuard] },
  { path: 'bookings', component: BookingsComponent, canActivate: [authGuard] },
  { path: 'role-requests', component: RoleRequestsComponent, canActivate: [authGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  { path: '', redirectTo: 'bookings', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' },
];
