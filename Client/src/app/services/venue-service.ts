import { inject, Injectable } from '@angular/core';
import { RestApiService } from './rest-api-service';
import { Venue } from '../models/Venue';
import { VenueWithBookings } from '../models/VenueWithBookings';

@Injectable({
  providedIn: 'root',
})
export class VenueService {
  private api = inject(RestApiService);

  async getVenueWithBookings(id: number): Promise<VenueWithBookings> {
    return await this.api.get<VenueWithBookings>(`/api/venues/${id}`);
  }

  async getVenueWithBookingsBySlug(slug: string): Promise<VenueWithBookings> {
    return await this.api.get<VenueWithBookings>(`/api/venues/slug/${slug}`);
  }

  async getVenues(): Promise<Venue[]> {
    return await this.api.get<Venue[]>('/api/venues');
  }
}
