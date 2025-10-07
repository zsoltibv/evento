import { VenueService } from './../../services/venue-service';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VenueWithBookings } from '../../models/VenueWithBookings';
import { CardModule } from 'primeng/card';
import { DatePickerModule } from 'primeng/datepicker';
import { TagModule } from 'primeng/tag';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { BookingService } from '../../services/booking-service';
import { MessageService } from 'primeng/api';
import { extractErrorMessages } from '../../utils/ApiError';

@Component({
  selector: 'app-venue-component',
  imports: [CardModule, DatePickerModule, TagModule, ButtonModule, FormsModule],
  templateUrl: './venue-component.html',
  styleUrl: './venue-component.scss',
})
export class VenueComponent {
  private activatedRoute = inject(ActivatedRoute);
  private venueService = inject(VenueService);
  private bookingService = inject(BookingService);
  private messageService = inject(MessageService);

  venue = signal<VenueWithBookings>({} as VenueWithBookings);
  venueId = signal<number>(0);

  startDate = signal<Date | null>(null);
  endDate = signal<Date | null>(null);

  ngOnInit() {
    this.activatedRoute.params.subscribe((params) => {
      this.venueId.set(params['id']);
    });

    this.loadVenueWithBookings();
  }

  async loadVenueWithBookings() {
    const result = await this.venueService.getVenueWithBookings(this.venueId());
    this.venue.set(result);
  }

  getDisabledDates() {
    return this.venue()?.bookings?.map((b) => new Date(b.startDate)) || [];
  }

  async submitBooking() {
    if (!this.startDate() || !this.endDate()) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Missing Dates',
        detail: 'Please select both start and end dates.',
      });
      return;
    }

    await this.bookingService.createBooking({
      venueId: this.venueId(),
      startDate: this.startDate()!,
      endDate: this.endDate()!,
    });

    this.messageService.add({
      severity: 'success',
      summary: 'Booking Submitted',
      detail: 'Your booking has been successfully submitted!',
    });

    this.loadVenueWithBookings();

    this.startDate.set(null);
    this.endDate.set(null);
  }
}
