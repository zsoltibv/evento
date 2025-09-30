import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environment';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class RestApiService {
  private http = inject(HttpClient);

  private buildUrl(endpoint: string): string {
    return `${environment.apiBaseUrl}${endpoint}`;
  }

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('jwt');
    return new HttpHeaders(token ? { Authorization: `Bearer ${token}` } : {});
  }

  get<T>(endpoint: string, options: object = {}): Promise<T> {
    return firstValueFrom(
      this.http.get<T>(this.buildUrl(endpoint), {
        headers: this.getHeaders(),
        ...options,
      })
    );
  }

  post<T>(endpoint: string, body: any, options: object = {}): Promise<T> {
    return firstValueFrom(
      this.http.post<T>(this.buildUrl(endpoint), body, {
        headers: this.getHeaders(),
        ...options,
      })
    );
  }

  put<T>(endpoint: string, body: any, options: object = {}): Promise<T> {
    return firstValueFrom(
      this.http.put<T>(this.buildUrl(endpoint), body, {
        headers: this.getHeaders(),
        ...options,
      })
    );
  }

  delete<T>(endpoint: string, options: object = {}): Promise<T> {
    return firstValueFrom(
      this.http.delete<T>(this.buildUrl(endpoint), {
        headers: this.getHeaders(),
        ...options,
      })
    );
  }
}
