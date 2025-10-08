import { BookingStatus } from '../components/enums/BookingStatus';

export interface UpdateBooking {
  startDate?: Date;
  endDate?: Date;
  venueId?: number;
  status?: BookingStatus;
}
