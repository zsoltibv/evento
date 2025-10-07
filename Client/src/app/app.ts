import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { NavbarComponent } from './components/shared/navbar-component/navbar-component';
import { ToastModule } from 'primeng/toast';
import { filter } from 'rxjs';
import { ConfirmDialog } from 'primeng/confirmdialog';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavbarComponent, ToastModule, CommonModule, ConfirmDialog],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  protected readonly title = signal('Evento');
  private readonly router = inject(Router);

  private readonly currentUrl = signal('');
  protected readonly isAuthPage = computed(() =>
    ['/login', '/register'].includes(this.currentUrl())
  );

  constructor() {
    this.router.events
      .pipe(filter((e) => e instanceof NavigationEnd))
      .subscribe((e: NavigationEnd) => {
        this.currentUrl.set(e.urlAfterRedirects);
      });
  }

  ngOnInit() {
    this.currentUrl.set(this.router.url);
  }
}
