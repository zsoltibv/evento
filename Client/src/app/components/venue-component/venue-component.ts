import { AuthService } from './../../services/auth-service';
import { VenueService } from './../../services/venue-service';
import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VenueWithBookings } from '../../models/VenueWithBookings';
import { CardModule } from 'primeng/card';
import { TagModule } from 'primeng/tag';
import { BookingCard } from '../booking-card/booking-card';
import { AddOrEditBookingCard } from '../add-or-edit-booking-card/add-or-edit-booking-card';
import { BookingStatusUpdate } from '../../models/BookingStatusUpdate';
import { BookingWithVenueName } from '../../models/BookingWithVenueName';

@Component({
  selector: 'app-venue-component',
  imports: [CardModule, TagModule, BookingCard, AddOrEditBookingCard],
  templateUrl: './venue-component.html',
  styleUrl: './venue-component.scss',
})
export class VenueComponent {
  private activatedRoute = inject(ActivatedRoute);
  private venueService = inject(VenueService);
  private authService = inject(AuthService);

  venue = signal<VenueWithBookings>({} as VenueWithBookings);
  slug = signal<string>('');

  ngOnInit() {
    this.activatedRoute.params.subscribe((params) => {
      const slug = params['slug'];
      if (slug) {
        this.slug.set(slug);
        this.loadVenueWithBookings();
      }
    });
  }

  protected readonly userBookings = computed<BookingWithVenueName[]>(() => {
    const userId = this.authService.userId();
    return this.venue()?.bookings?.filter((b) => b.userId === userId) || [];
  });

  async loadVenueWithBookings() {
    const result = await this.venueService.getVenueWithBookingsBySlug(this.slug());
    this.venue.set(result);
  }

  protected onCancel(booking: BookingStatusUpdate) {
    this.loadVenueWithBookings();
  }
}
