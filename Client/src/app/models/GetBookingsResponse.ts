import { BookingWithVenueName } from './BookingWithVenueName';

export interface GetBookingsResponse {
  userBookings: BookingWithVenueName[];
  venueBookings: BookingWithVenueName[];
}
