import { Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login-component/login-component';
import { RegisterComponent } from './components/auth/register-component/register-component';
import { VenuesComponent } from './components/venues-component/venues-component';
import { authGuard } from './guards/auth-guard';
import { VenueComponent } from './components/venue-component/venue-component';
import { BookingsComponent } from './components/bookings-component/bookings-component';
import { RoleRequestsComponent } from './components/role-requests-component/role-requests-component';
import { ChatComponent } from './components/chat-component/chat-component';
import { BookingComponent } from './components/booking-component/booking-component';
import { PayVenueBooking } from './components/payment/pay-venue-booking/pay-venue-booking';

export const routes: Routes = [
  { path: 'venues', component: VenuesComponent, canActivate: [authGuard] },
  { path: 'venues/:slug', component: VenueComponent, canActivate: [authGuard] },
  { path: 'bookings', component: BookingsComponent, canActivate: [authGuard] },
  { path: 'bookings/:id', component: BookingComponent, canActivate: [authGuard] },
  { path: 'role-requests', component: RoleRequestsComponent, canActivate: [authGuard] },
  { path: 'chat', component: ChatComponent, canActivate: [authGuard] },
  { path: 'pay/:id', component: PayVenueBooking, canActivate: [authGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  { path: '', redirectTo: 'bookings', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' },
];
