import { VenueService } from '../../services/venue-service';
import { Component, computed, effect, inject, input, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DatePickerModule } from 'primeng/datepicker';
import { VenueWithBookings } from '../../models/VenueWithBookings';
import { Booking } from '../../models/Booking';
import { BookingService } from '../../services/booking-service';
import { MessageService } from 'primeng/api';
import { BookingWithVenueName } from '../../models/BookingWithVenueName';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-or-edit-booking-card',
  imports: [DatePickerModule, ButtonModule, FormsModule, CardModule, CommonModule],
  templateUrl: './add-or-edit-booking-card.html',
  styleUrl: './add-or-edit-booking-card.scss',
})
export class AddOrEditBookingCard {
  private bookingService = inject(BookingService);
  private messageService = inject(MessageService);
  private venueService = inject(VenueService);

  venue = input<VenueWithBookings | null>(null);
  booking = input<BookingWithVenueName | null>(null);

  onBookingAdded = output<void>();
  onBookingEdited = output<BookingWithVenueName>();

  startDate = signal<Date | null>(null);
  endDate = signal<Date | null>(null);
  currentVenue = signal<VenueWithBookings | null>(null);

  constructor() {
    effect(() => {
      const venue = this.venue();
      const id = this.booking()?.venueId;

      if (venue) {
        this.currentVenue.set(venue);
      } else if (id) {
        this.fetchVenueFromApi(id);
        this.startDate.set(new Date(this.booking()!.startDate));
        this.endDate.set(new Date(this.booking()!.endDate));
      }
    });
  }

  isEditing = computed(() => !!this.booking());

  protected readonly disabledDates = computed<Date[]>(() => {
    const bookings = this.currentVenue()?.bookings || [];
    const dates: Date[] = [];

    bookings.forEach((b: Booking) => {
      const start = new Date(b.startDate);
      const end = new Date(b.endDate);

      const fullStart = new Date(start);
      const fullEnd = new Date(end);

      fullStart.setDate(fullStart.getDate() + 1);
      fullStart.setHours(0, 0, 0, 0);

      fullEnd.setHours(0, 0, 0, 0);
      let current = fullStart;

      while (current < fullEnd) {
        dates.push(new Date(current));
        current.setDate(current.getDate() + 1);
      }
    });

    return dates;
  });

  protected readonly minDate = computed(() => {
    const d = new Date();
    d.setHours(0, 0, 0, 0);
    return d;
  });

  private async fetchVenueFromApi(id: number): Promise<void> {
    const venue: VenueWithBookings = await this.venueService.getVenueWithBookings(id);
    this.currentVenue.set(venue);
  }

  async submitBooking(): Promise<void> {
    if (!this.startDate() || !this.endDate()) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Missing Dates',
        detail: 'Please select both start and end dates.',
      });
      return;
    }

    if (this.isEditing()) {
      await this.editBooking();
    } else {
      await this.addBooking();
    }

    this.onBookingAdded.emit();
    this.startDate.set(null);
    this.endDate.set(null);
  }

  async addBooking(): Promise<void> {
    await this.bookingService.createBooking({
      venueId: this.currentVenue()!.id,
      startDate: this.startDate()!,
      endDate: this.endDate()!,
    });

    this.messageService.add({
      severity: 'success',
      summary: 'Booking Submitted',
      detail: 'Your booking has been successfully submitted!',
    });
  }

  async editBooking(): Promise<void> {
    const updatedBooking: BookingWithVenueName = {
      ...this.booking()!,
      startDate: this.startDate()!,
      endDate: this.endDate()!,
    };

    await this.bookingService.updateBooking(this.booking()!.id, {
      startDate: updatedBooking.startDate,
      endDate: updatedBooking.endDate,
    });

    this.messageService.add({
      severity: 'success',
      summary: 'Booking updated',
      detail: 'Your booking has been successfully updated!',
    });

    this.onBookingEdited.emit(updatedBooking);
  }
}
