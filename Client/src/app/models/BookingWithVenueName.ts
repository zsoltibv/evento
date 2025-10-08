import { BookingStatus } from '../components/enums/BookingStatus';

export interface BookingWithVenueName {
  id: number;
  userId: string;
  startDate: Date;
  endDate: Date;
  bookingDate: Date;
  status: BookingStatus;
  venueId: number;
  venueName: string;
}
