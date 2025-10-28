import { BookingStatus } from '../components/enums/BookingStatus';
import { User } from './User';
import { Venue } from './Venue';

export interface BookingDetails {
  id: number;
  userId: string;
  startDate: Date;
  endDate: Date;
  bookingDate: Date;
  status: BookingStatus;
  venue: Venue;
  user: User;
}
