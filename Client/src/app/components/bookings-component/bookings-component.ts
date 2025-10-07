import { Component, inject, signal } from '@angular/core';
import { BookingService } from '../../services/booking-service';
import { Booking } from '../../models/Booking';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { BookingDatePipe } from '../../pipe/booking-date-pipe';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-bookings-component',
  imports: [BookingDatePipe, CardModule, ButtonModule],
  standalone: true,
  templateUrl: './bookings-component.html',
  styleUrl: './bookings-component.scss',
})
export class BookingsComponent {
  private bookingService = inject(BookingService);
  private messageService = inject(MessageService);

  bookings = signal<Booking[]>([]);

  async ngOnInit() {
    this.loadBookings();
  }

  async loadBookings() {
    const data = await this.bookingService.getBookings();
    this.bookings.set(data);
  }

  async deleteBooking(id: number) {
    try {
      await this.bookingService.deleteBooking(id);
      this.bookings.update((current) => current.filter((b) => b.id !== id));

      this.messageService.add({
        severity: 'success',
        summary: 'Deleted',
        detail: `Booking #${id} was deleted successfully`,
      });
    } catch (error) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: `Failed to delete booking #${id}`,
      });
      console.error(error);
    }
  }
}
