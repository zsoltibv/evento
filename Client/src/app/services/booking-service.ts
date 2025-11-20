import { inject, Injectable } from '@angular/core';
import { RestApiService } from './rest-api-service';
import { Booking } from '../models/Booking';
import { CreateBooking } from '../models/CreateBooking';
import { UpdateBooking } from '../models/UpdateBooking';
import { GetBookingsResponse } from '../models/GetBookingsResponse';
import { BookingDetails } from '../models/BookingDetails';
import { UpdateBookingPayment } from '../models/UpdateBookingPayment';

@Injectable({
  providedIn: 'root',
})
export class BookingService {
  private api = inject(RestApiService);

  async getBookings(): Promise<GetBookingsResponse> {
    return await this.api.get<GetBookingsResponse>('/api/bookings');
  }

  async getBooking(id: number): Promise<BookingDetails> {
    return await this.api.get<BookingDetails>(`/api/bookings/${id}`);
  }

  async createBooking(booking: CreateBooking): Promise<Booking> {
    return await this.api.post<Booking>('/api/bookings', booking);
  }

  async updateBooking(id: number, booking: Partial<UpdateBooking>): Promise<Booking> {
    return await this.api.put<Booking>(`/api/bookings/${id}`, booking);
  }

  async deleteBooking(id: number): Promise<void> {
    return await this.api.delete<void>(`/api/bookings/${id}`);
  }

  async updateBookingPayment(id: number, payment: UpdateBookingPayment): Promise<Booking> {
    return await this.api.put<Booking>(`/api/bookings/${id}/payment`, payment);
  }
}
