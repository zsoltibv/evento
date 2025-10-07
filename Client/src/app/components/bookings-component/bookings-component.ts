import { Component, inject, signal } from '@angular/core';
import { BookingService } from '../../services/booking-service';
import { Booking } from '../../models/Booking';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { BookingCard } from '../booking-card/booking-card';

@Component({
  selector: 'app-bookings-component',
  imports: [CardModule, ButtonModule, BookingCard],
  standalone: true,
  templateUrl: './bookings-component.html',
  styleUrl: './bookings-component.scss',
})
export class BookingsComponent {
  private bookingService = inject(BookingService);
  bookings = signal<Booking[]>([]);

  async ngOnInit() {
    this.loadBookings();
  }

  async loadBookings() {
    const data = await this.bookingService.getBookings();
    this.bookings.set(data);
  }

  protected removeBooking(deletedId: number) {
    this.bookings.set(this.bookings().filter((b) => b.id !== deletedId));
  }
}
