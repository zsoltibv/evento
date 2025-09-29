import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../../../services/auth-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-component',
  imports: [FormsModule, ReactiveFormsModule, InputTextModule, PasswordModule, ButtonModule],
  templateUrl: './login-component.html',
  styleUrl: './login-component.scss',
  standalone: true,
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  email = signal('');
  password = signal('');

  isValid = computed(() => {
    return this.email().trim() !== '' && this.password().trim() !== '';
  });

  async onSubmit() {
    if (!this.isValid()) {
      console.warn('Form is invalid');
    }

    try {
      const response = await this.authService.login(this.email(), this.password());
      console.log('Login successful', response);
      this.router.navigate(['/home']);
    } catch (err: any) {
      console.error('Login failed', err);
    }
  }
}
