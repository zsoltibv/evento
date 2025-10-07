import { inject, Injectable } from '@angular/core';
import { RestApiService } from './rest-api-service';
import { Booking } from '../models/Booking';
import { CreateBooking } from '../models/CreateBooking';
import { UpdateBooking } from '../models/UpdateBooking';

@Injectable({
  providedIn: 'root',
})
export class BookingService {
  private api = inject(RestApiService);

  async getBookings(): Promise<Booking[]> {
    return await this.api.get<Booking[]>('/api/bookings');
  }

  async getBooking(id: number): Promise<Booking[]> {
    return await this.api.get<Booking[]>(`/api/bookings/${id}`);
  }

  async createBooking(booking: CreateBooking): Promise<Booking> {
    return await this.api.post<Booking>('/api/bookings', booking);
  }

  async updateBooking(id: number, booking: UpdateBooking): Promise<Booking> {
    return await this.api.put<Booking>(`/api/bookings/${id}`, booking);
  }

  async deleteBooking(id: number): Promise<void> {
    return await this.api.delete<void>(`/api/bookings/${id}`);
  }
}
