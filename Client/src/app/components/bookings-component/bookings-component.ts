import { Component, computed, inject, signal } from '@angular/core';
import { BookingService } from '../../services/booking-service';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { BookingCard } from '../booking-card/booking-card';
import { BookingStatusUpdate } from '../../models/BookingStatusUpdate';
import { BookingWithVenueName } from '../../models/BookingWithVenueName';
import { TabsModule } from 'primeng/tabs';
import { AuthService } from '../../services/auth-service';
import { DatePickerModule } from 'primeng/datepicker';
import { SelectModule } from 'primeng/select';
import { CheckboxModule } from 'primeng/checkbox';
import { FormsModule } from '@angular/forms';
import { exportBookingsToExcel } from '../../utils/excel-export.util';

@Component({
  selector: 'app-bookings-component',
  imports: [
    CardModule,
    ButtonModule,
    BookingCard,
    TabsModule,
    DatePickerModule,
    SelectModule,
    CheckboxModule,
    FormsModule,
  ],
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

  // filters
  filters = signal({
    fromDate: undefined as Date | undefined,
    toDate: undefined as Date | undefined,
    status: undefined as string | undefined,
    isPaid: undefined as boolean | undefined,
  });

  paidOptions = [
    { label: 'All', value: undefined },
    { label: 'Paid', value: true },
    { label: 'Unpaid', value: false },
  ];

  statusOptions = [
    { label: 'Pending', value: 'Pending' },
    { label: 'Approved', value: 'Approved' },
    { label: 'Cancelled', value: 'Cancelled' },
    { label: 'Rejected', value: 'Rejected' },
  ];

  async ngOnInit() {
    this.loadBookings();
  }

  async loadBookings(): Promise<void> {
    const data = await this.bookingService.getBookings({
      fromDate: this.filters().fromDate,
      toDate: this.filters().toDate,
      status: this.filters().status,
      isPaid: this.filters().isPaid,
    });

    this.bookings.set(data.userBookings);
    this.venueBookings.set(data.venueBookings);
  }

  clearFilters(): void {
    this.filters.set({
      fromDate: undefined,
      toDate: undefined,
      status: undefined,
      isPaid: undefined,
    });

    this.loadBookings();
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

  downloadExcel() {
    const allBookings = [...this.bookings(), ...this.venueBookings()];

    if (!allBookings.length) {
      return;
    }

    exportBookingsToExcel(allBookings, 'Bookings');
  }
}
