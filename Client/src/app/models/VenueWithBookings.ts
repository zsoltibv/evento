import { Booking } from './Booking';
import { BookingWithVenueName } from './BookingWithVenueName';

export interface VenueWithBookings {
  id: number;
  name: string;
  description?: string;
  location: string;
  capacity: number;
  imageUrl?: string;
  pricePerHour: number;
  bookings: BookingWithVenueName[];
}
