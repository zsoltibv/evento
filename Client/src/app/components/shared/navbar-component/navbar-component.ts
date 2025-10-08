import { Component, computed, inject, ViewChild } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { MenubarModule } from 'primeng/menubar';
import { AuthService } from '../../../services/auth-service';
import { ButtonModule } from 'primeng/button';
import { AvatarModule } from 'primeng/avatar';
import { UserTokenInfo } from '../../../models/UserTokenInfo';
import { SelectModule } from 'primeng/select';
import { Menu, MenuModule } from 'primeng/menu';

@Component({
  selector: 'app-navbar-component',
  imports: [MenubarModule, RouterModule, ButtonModule, AvatarModule, SelectModule, MenuModule],
  templateUrl: './navbar-component.html',
  styleUrl: './navbar-component.scss',
})
export class NavbarComponent {
  @ViewChild('userMenu') userMenu!: Menu;

  private authService = inject(AuthService);
  private router = inject(Router);

  protected menuItems = computed<MenuItem[]>(() => {
    if (!this.authService.jwtToken()) return [];

    if (this.authService.isUser()) {
      return [
        { label: 'Venues', routerLink: '/venues' },
        { label: 'Bookings', routerLink: '/bookings' },
      ];
    }

    if (this.authService.isAdmin()) {
      return [{ label: 'Bookings', routerLink: '/bookings' }];
    }

    return [];
  });

  userMenuItems: MenuItem[] = [
    { separator: true },
    {
      label: 'My Bookings',
      icon: 'pi pi-calendar',
      routerLink: '/bookings',
    },
    {
      label: 'Logout',
      icon: 'pi pi-sign-out',
      command: () => this.logout(),
    },
  ];

  protected isLoggedIn = computed(() => {
    return !!this.authService.jwtToken();
  });

  protected user = computed<UserTokenInfo | null>(() => {
    return this.authService.userTokenInfo();
  });

  protected toggleUserMenu(event: Event) {
    this.userMenu.toggle(event);
  }

  protected logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
