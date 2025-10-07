import { AuthService } from './../../../services/auth-service';
import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';
import { LoginResponse } from '../../../models/LoginResponse';

@Component({
  selector: 'app-register-component',
  imports: [FormsModule, InputTextModule, PasswordModule, ButtonModule],
  templateUrl: './register-component.html',
  styleUrl: './register-component.scss',
})
export class RegisterComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  username = signal('');
  email = signal('');
  password = signal('');
  confirmPassword = signal('');

  isValid = computed(() => {
    return (
      this.username().trim() !== '' &&
      this.email().trim() !== '' &&
      this.password().trim() !== '' &&
      this.password() === this.confirmPassword()
    );
  });

  async onSubmit() {
    if (!this.isValid()) {
      console.warn('Form is invalid');
      return;
    }

    try {
      const response = await this.authService.register(
        this.username(),
        this.email(),
        this.password(),
        this.confirmPassword()
      );
      console.log('Register successful:', response);
      this.router.navigate(['/venues']);
    } catch (err: any) {
      console.error('Register failed', err);
    }
  }
}
