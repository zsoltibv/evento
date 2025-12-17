import { inject, Injectable } from '@angular/core';
import { RestApiService } from './rest-api-service';
import { Booking } from '../models/Booking';
import { CreateBooking } from '../models/CreateBooking';
import { UpdateBooking } from '../models/UpdateBooking';
import { GetBookingsResponse } from '../models/GetBookingsResponse';
import { BookingDetails } from '../models/BookingDetails';
import { UpdateBookingPayment } from '../models/UpdateBookingPayment';
import { BookingFilter } from '../models/BookingFilter';

@Injectable({
  providedIn: 'root',
})
export class BookingService {
  private api = inject(RestApiService);

  async getBookings(filter?: BookingFilter): Promise<GetBookingsResponse> {
    const params: Record<string, any> = {};
    if (filter?.fromDate) params['fromDate'] = filter.fromDate.toISOString();

    if (filter?.toDate) params['toDate'] = filter.toDate.toISOString();

    if (filter?.status) params['status'] = filter.status;

    if (filter?.venueId) params['venueId'] = filter.venueId;

    if (filter?.isPaid !== undefined) params['isPaid'] = filter.isPaid;

    return await this.api.get<GetBookingsResponse>('/api/bookings', { params });
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
