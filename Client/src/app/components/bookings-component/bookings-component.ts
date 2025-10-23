import { Component, computed, inject, signal } from '@angular/core';
import { BookingService } from '../../services/booking-service';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { BookingCard } from '../booking-card/booking-card';
import { BookingStatusUpdate } from '../../models/BookingStatusUpdate';
import { BookingWithVenueName } from '../../models/BookingWithVenueName';
import { GetBookingsResponse } from '../../models/GetBookingsResponse';
import { TabsModule } from 'primeng/tabs';
import { AuthService } from '../../services/auth-service';

@Component({
  selector: 'app-bookings-component',
  imports: [CardModule, ButtonModule, BookingCard, TabsModule],
  standalone: true,
  templateUrl: './bookings-component.html',
  styleUrl: './bookings-component.scss',
})
export class BookingsComponent {
  private bookingService = inject(BookingService);
  private authService = inject(AuthService);

  bookings = signal<BookingWithVenueName[]>([]);
  venueBookings = signal<BookingWithVenueName[]>([]);

  isUser = computed(() => this.authService.isUser());
  activeTab = computed(() => {
    return this.authService.isUser() ? '0' : '1';
  });

  async ngOnInit() {
    this.loadBookings();
  }

  async loadBookings(): Promise<void> {
    const data: GetBookingsResponse = await this.bookingService.getBookings();
    this.bookings.set(data.userBookings);
    this.venueBookings.set(data.venueBookings);
  }

  protected onCancelBookings(update: BookingStatusUpdate): void {
    this.bookings.set(
      this.bookings().map((b) => (b.id === update.id ? { ...b, status: update.status } : b))
    );
  }

  protected onCancelVenueBookings(update: BookingStatusUpdate): void {
    this.venueBookings.set(
      this.venueBookings().map((b) => (b.id === update.id ? { ...b, status: update.status } : b))
    );
  }
}
