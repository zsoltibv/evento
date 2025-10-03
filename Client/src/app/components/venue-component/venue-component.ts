import { VenueService } from './../../services/venue-service';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VenueWithBookings } from '../../models/VenueWithBookings';
import { CardModule } from 'primeng/card';
import { DatePickerModule } from 'primeng/datepicker';
import { TagModule } from 'primeng/tag';
import { DatePipe } from '@angular/common';
import { Button, ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-venue-component',
  imports: [CardModule, DatePickerModule, TagModule, DatePipe, ButtonModule],
  templateUrl: './venue-component.html',
  styleUrl: './venue-component.scss',
})
export class VenueComponent {
  private activatedRoute = inject(ActivatedRoute);
  private venueService = inject(VenueService);

  venue = signal<VenueWithBookings>({} as VenueWithBookings);
  venueId = signal<number>(0);

  ngOnInit() {
    this.activatedRoute.params.subscribe((params) => {
      this.venueId.set(params['id']);
    });

    this.loadVenueWithBookings();
  }

  async loadVenueWithBookings() {
    const result = await this.venueService.getVenueWithBookings(this.venueId());
    console.log(result);

    this.venue.set(result);
  }

  protected getDisabledDates(): Date[] {
    return this.venue().bookings.map((b) => new Date(b.startDate));
  }

  protected submitBooking() {
    alert('Booking submitted!');
  }
}
