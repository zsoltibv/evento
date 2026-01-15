export interface StatisticsDto {
  bookingsCount: number;
  venuesCount: number;
  totalRevenue: number;
  weeklyRevenue: number[];
  venueStatistics?: VenueStatisticDto[];
}

export interface VenueStatisticDto {
  venueId: number;
  venueName: string;
  bookingsCount: number;
  totalRevenue: number;
}
