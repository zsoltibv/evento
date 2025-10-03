import { Booking } from './Booking';

export interface VenueWithBookings {
  id: number;
  name: string;
  description?: string;
  location: string;
  capacity: number;
  imageUrl?: string;
  bookings: Booking[];
}
