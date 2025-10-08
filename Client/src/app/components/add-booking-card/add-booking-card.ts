import { Component, computed, inject, input, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DatePickerModule } from 'primeng/datepicker';
import { VenueWithBookings } from '../../models/VenueWithBookings';
import { Booking } from '../../models/Booking';
import { BookingService } from '../../services/booking-service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-add-booking-card',
  imports: [DatePickerModule, ButtonModule, FormsModule, CardModule],
  templateUrl: './add-booking-card.html',
  styleUrl: './add-booking-card.scss',
})
export class AddBookingCard {
  private bookingService = inject(BookingService);
  private messageService = inject(MessageService);

  venue = input.required<VenueWithBookings>();
  onBookingAdded = output<void>();

  startDate = signal<Date | null>(null);
  endDate = signal<Date | null>(null);

  protected readonly disabledDates = computed<Date[]>(() => {
    const bookings = this.venue()?.bookings || [];
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
      venueId: this.venue().id,
      startDate: this.startDate()!,
      endDate: this.endDate()!,
    });

    this.messageService.add({
      severity: 'success',
      summary: 'Booking Submitted',
      detail: 'Your booking has been successfully submitted!',
    });

    this.onBookingAdded.emit();
    this.startDate.set(null);
    this.endDate.set(null);
  }
}
