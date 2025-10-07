export interface Booking {
  id: number;
  userId: string;
  startDate: Date;
  endDate: Date;
  bookingDate: Date;
  status: string;
  venueId: number;
}
