import { computed, inject, Injectable, signal } from '@angular/core';
import { LoginResponse } from '../models/LoginResponse';
import { RestApiService } from './rest-api-service';
import { jwtDecode } from 'jwt-decode';
import { UserTokenInfo } from '../models/UserTokenInfo';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  jwtToken = signal<string | null>(null);
  private api = inject(RestApiService);

  constructor() {
    const storedToken = localStorage.getItem('jwt');
    if (storedToken) {
      this.jwtToken.set(storedToken);
    }
  }

  readonly userId = computed(() => {
    const token = this.jwtToken();
    if (!token) return null;
    try {
      const payload = jwtDecode(token);
      return payload.sub;
    } catch {
      return null;
    }
  });

  readonly userTokenInfo = computed<UserTokenInfo | null>(() => {
    const token = this.jwtToken();
    if (!token) return null;

    try {
      const payload: any = jwtDecode(token);
      return {
        id: payload.sub,
        username: payload.given_name,
        email: payload.email,
        roles: payload.role ? [payload.role] : [],
      };
    } catch {
      return null;
    }
  });

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
