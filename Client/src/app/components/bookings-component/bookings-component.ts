import { Component, inject, signal } from '@angular/core';
import { BookingService } from '../../services/booking-service';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { BookingCard } from '../booking-card/booking-card';
import { BookingStatusUpdate } from '../../models/BookingStatusUpdate';
import { BookingWithVenueName } from '../../models/BookingWithVenueName';

@Component({
  selector: 'app-bookings-component',
  imports: [CardModule, ButtonModule, BookingCard],
  standalone: true,
  templateUrl: './bookings-component.html',
  styleUrl: './bookings-component.scss',
})
export class BookingsComponent {
  private bookingService = inject(BookingService);
  bookings = signal<BookingWithVenueName[]>([]);

  async ngOnInit() {
    this.loadBookings();
  }

  async loadBookings(): Promise<void> {
    const data = await this.bookingService.getBookings();
    this.bookings.set(data);
  }

  protected onCancel(update: BookingStatusUpdate): void {
    this.bookings.set(
      this.bookings().map((b) => (b.id === update.id ? { ...b, status: update.status } : b))
    );
  }
}
