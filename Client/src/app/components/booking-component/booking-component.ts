import { Component, inject, signal } from '@angular/core';
import { BookingDetails } from '../../models/BookingDetails';
import { ActivatedRoute, Router } from '@angular/router';
import { BookingService } from '../../services/booking-service';
import { CommonModule } from '@angular/common';
import { BookingDatePipe } from '../../pipe/booking-date-pipe';
import { PanelModule } from 'primeng/panel';
import { ChatWidget } from '../chat-widget/chat-widget';

@Component({
  selector: 'app-booking-component',
  imports: [CommonModule, BookingDatePipe, PanelModule, ChatWidget],
  templateUrl: './booking-component.html',
  styleUrl: './booking-component.scss',
})
export class BookingComponent {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private bookingService = inject(BookingService);

  booking = signal<BookingDetails | null>(null);

  async ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!id) return;

    try {
      const result = await this.bookingService.getBooking(id);
      this.booking.set(result);
    } catch (error) {
      console.error('Error loading booking details:', error);
    }
  }

  protected goToVenue() {
    this.router.navigate(['/venues', this.booking()!.venue.slug]);
  }
}
