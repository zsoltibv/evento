import { Component, computed, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { MenubarModule } from 'primeng/menubar';
import { AuthService } from '../../../services/auth-service';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-navbar-component',
  imports: [MenubarModule, RouterModule, ButtonModule],
  templateUrl: './navbar-component.html',
  styleUrl: './navbar-component.scss',
})
export class NavbarComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  protected menuItems = computed<MenuItem[]>(() => {
    const loggedIn = !!this.authService.jwtToken();
    return loggedIn
      ? [
          { label: 'Venues', routerLink: '/venues' },
          { label: 'Bookings', routerLink: '/bookings' },
        ]
      : [];
  });

  protected isLoggedIn = computed(() => {
    return !!this.authService.jwtToken();
  });

  protected logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
