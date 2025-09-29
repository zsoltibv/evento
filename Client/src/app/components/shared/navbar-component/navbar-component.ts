import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { MenubarModule } from 'primeng/menubar';

@Component({
  selector: 'app-navbar-component',
  imports: [MenubarModule, RouterModule],
  templateUrl: './navbar-component.html',
  styleUrl: './navbar-component.scss',
})
export class NavbarComponent {
  items: MenuItem[] = [];

  constructor() {
    this.items = [
      { label: 'Login', routerLink: '/login' },
      { label: 'Register', routerLink: '/register' },
    ];
  }
}
