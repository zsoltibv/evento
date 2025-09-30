import { inject, Injectable, signal } from '@angular/core';
import { LoginResponse } from '../models/LoginResponse';
import { RestApiService } from './rest-api-service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  jwtToken = signal<string | null>(null);
  private api = inject(RestApiService);

  constructor() {
    if (typeof localStorage == 'undefined') {
      return;
    }

    const storedToken = localStorage.getItem('jwt');
    if (storedToken) {
      this.jwtToken.set(storedToken);
    }
  }

  async login(email: string, password: string): Promise<LoginResponse> {
    const response = await this.api.post<LoginResponse>('/api/auth/login', { email, password });
    localStorage.setItem('jwt', response.token);
    this.jwtToken.set(response.token);
    return response;
  }

  async register(
    username: string,
    email: string,
    password: string,
    confirmPassword: string
  ): Promise<LoginResponse> {
    const response = await this.api.post<LoginResponse>('/api/auth/register', {
      username,
      email,
      password,
      confirmPassword,
    });
    localStorage.setItem('jwt', response.token);
    this.jwtToken.set(response.token);
    return response;
  }

  logout() {
    localStorage.removeItem('jwt');
    this.jwtToken.set(null);
  }

  getToken() {
    return this.jwtToken();
  }
}
