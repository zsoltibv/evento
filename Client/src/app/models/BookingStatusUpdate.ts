import { BookingStatus } from '../components/enums/BookingStatus';

export interface BookingStatusUpdate {
  id: number;
  status: BookingStatus;
}
