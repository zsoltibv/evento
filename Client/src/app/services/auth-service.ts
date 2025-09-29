import { environment } from './../../../environment';
import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { LoginResponse } from '../models/LoginResponse';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  jwtToken = signal<string | null>(null);

  private http = inject(HttpClient);

  login(email: string, password: string) {
    this.http
      .post<LoginResponse>(`${environment.apiBaseUrl}/api/auth/login`, { email, password })
      .subscribe({
        next: (response) => {
          localStorage.setItem('jwt', response.token);
          this.jwtToken.set(response.token);
        },
        error: (err) => {
          console.error('Login failed', err);
        },
      });
  }

  register(username: string, email: string, password: string, confirmPassword: string) {
    this.http
      .post<LoginResponse>(`${environment.apiBaseUrl}/api/auth/register`, {
        username,
        email,
        password,
        confirmPassword,
      })
      .subscribe({
        next: (response) => {
          localStorage.setItem('jwt', response.token);
          this.jwtToken.set(response.token);
        },
        error: (err) => {
          console.error('Register failed', err);
        },
      });
  }

  logout() {
    localStorage.removeItem('jwt');
    this.jwtToken.set(null);
  }

  getToken() {
    return this.jwtToken();
  }
}
