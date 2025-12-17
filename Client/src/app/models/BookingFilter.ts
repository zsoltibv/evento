export interface BookingFilter {
  fromDate?: Date;
  toDate?: Date;
  status?: string;
  venueId?: number;
  isPaid?: boolean;
}
