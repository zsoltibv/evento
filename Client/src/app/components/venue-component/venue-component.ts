import { AuthService } from './../../services/auth-service';
import { VenueService } from './../../services/venue-service';
import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VenueWithBookings } from '../../models/VenueWithBookings';
import { CardModule } from 'primeng/card';
import { TagModule } from 'primeng/tag';
import { Booking } from '../../models/Booking';
import { BookingCard } from '../booking-card/booking-card';
import { AddBookingCard } from '../add-booking-card/add-booking-card';
import { BookingStatusUpdate } from '../../models/BookingStatusUpdate';
import { BookingWithVenueName } from '../../models/BookingWithVenueName';

@Component({
  selector: 'app-venue-component',
  imports: [CardModule, TagModule, BookingCard, AddBookingCard],
  templateUrl: './venue-component.html',
  styleUrl: './venue-component.scss',
})
export class VenueComponent {
  private activatedRoute = inject(ActivatedRoute);
  private venueService = inject(VenueService);
  private authService = inject(AuthService);

  venue = signal<VenueWithBookings>({} as VenueWithBookings);
  venueId = signal<number>(0);

  ngOnInit() {
    this.activatedRoute.params.subscribe((params) => {
      this.venueId.set(params['id']);
    });

    this.loadVenueWithBookings();
  }

  protected readonly userBookings = computed<BookingWithVenueName[]>(() => {
    const userId = this.authService.userId();
    return this.venue()?.bookings?.filter((b) => b.userId === userId) || [];
  });

  async loadVenueWithBookings() {
    const result = await this.venueService.getVenueWithBookings(this.venueId());
    this.venue.set(result);
  }

  protected onCancel(booking: BookingStatusUpdate) {
    this.loadVenueWithBookings();
  }
}
