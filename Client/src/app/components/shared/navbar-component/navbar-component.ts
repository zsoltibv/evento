import { Component, computed, inject, OnInit, ViewChild } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { MenubarModule } from 'primeng/menubar';
import { AuthService } from '../../../services/auth-service';
import { ButtonModule } from 'primeng/button';
import { AvatarModule } from 'primeng/avatar';
import { UserTokenInfo } from '../../../models/UserTokenInfo';
import { SelectModule } from 'primeng/select';
import { Menu, MenuModule } from 'primeng/menu';
import { ChatService } from '../../../services/chat-service';

@Component({
  selector: 'app-navbar-component',
  imports: [MenubarModule, RouterModule, ButtonModule, AvatarModule, SelectModule, MenuModule],
  templateUrl: './navbar-component.html',
  styleUrl: './navbar-component.scss',
})
export class NavbarComponent {
  @ViewChild('userMenu') userMenu!: Menu;

  private authService = inject(AuthService);
  private chatService = inject(ChatService);
  private router = inject(Router);

  protected menuItems = computed<MenuItem[]>(() => {
    if (!this.authService.jwtToken()) return [];

    if (this.authService.isUser()) {
      return [
        { label: 'Venues', routerLink: '/venues' },
        { label: 'Bookings', routerLink: '/bookings' },
        { label: 'Role Requests', routerLink: '/role-requests' },
      ];
    }

    if (this.authService.isAdmin()) {
      return [
        { label: 'Bookings', routerLink: '/bookings' },
        { label: 'Role Requests', routerLink: '/role-requests' },
      ];
    }

    return [];
  });

  protected userMenuItems = computed<MenuItem[]>(() => {
    const unreadCount = this.chatService.unreadMessagesCount();

    return [
      { separator: true },
      {
        label: 'My Bookings',
        icon: 'pi pi-calendar',
        routerLink: '/bookings',
      },
      {
        label: `Chat${unreadCount > 0 ? ` (${unreadCount})` : ''}`,
        icon: 'pi pi-comments',
        routerLink: '/chat',
        command: () => this.chatService.resetUnreadMessages(),
      },
      {
        label: 'Logout',
        icon: 'pi pi-sign-out',
        command: () => this.logout(),
      },
    ];
  });

  protected isLoggedIn = computed(() => {
    return !!this.authService.jwtToken();
  });

  protected formattedRoles = computed(() => {
    const roles = this.authService.roles();
    return Array.isArray(roles) ? roles.join(', ') : roles;
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
