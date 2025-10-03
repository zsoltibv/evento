import { Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login-component/login-component';
import { RegisterComponent } from './components/auth/register-component/register-component';
import { HomepageComponent } from './components/homepage-component/homepage-component';
import { authGuard } from './guards/auth-guard';
import { VenueComponent } from './components/venue-component/venue-component';

export const routes: Routes = [
  { path: 'home', component: HomepageComponent, canActivate: [authGuard] },
  { path: 'venue/:id', component: VenueComponent, canActivate: [authGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' },
];
