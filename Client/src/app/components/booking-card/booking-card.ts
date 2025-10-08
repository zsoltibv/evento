import { Component, inject, input, output } from '@angular/core';
import { BookingDatePipe } from '../../pipe/booking-date-pipe';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Booking } from '../../models/Booking';
import { BookingService } from '../../services/booking-service';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { BookingStatus } from '../enums/BookingStatus';
import { UpdateBooking } from '../../models/UpdateBooking';
import { BookingStatusUpdate } from '../../models/BookingStatusUpdate';
import { BookingWithVenueName } from '../../models/BookingWithVenueName';

@Component({
  selector: 'app-booking-card',
  imports: [BookingDatePipe, CardModule, ButtonModule, ConfirmDialogModule],
  templateUrl: './booking-card.html',
  styleUrl: './booking-card.scss',
})
export class BookingCard {
  booking = input.required<BookingWithVenueName>();
  onCancel = output<BookingStatusUpdate>();

  private messageService = inject(MessageService);
  private bookingService = inject(BookingService);
  private confirmationService = inject(ConfirmationService);

  protected confirmCancelBooking(id: number) {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to cancel this booking?',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      rejectLabel: 'Cancel',
      accept: async () => await this.cancelBooking(id),
    });
  }

  async cancelBooking(id: number) {
    try {
      const updatedBooking: Partial<UpdateBooking> = {
        status: BookingStatus.Cancelled,
      };

      await this.bookingService.updateBooking(id, updatedBooking);
      this.onCancel.emit({
        id: id,
        status: updatedBooking.status as BookingStatus,
      } as BookingStatusUpdate);

      this.messageService.add({
        severity: 'success',
        summary: 'Deleted',
        detail: `Booking #${id} was cancelled successfully`,
      });
    } catch (error) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: `Failed to cancel booking #${id}`,
      });
    }
  }

  async reopenBooking(id: number) {
    try {
      const updatedBooking: Partial<UpdateBooking> = {
        status: BookingStatus.Pending,
      };

      await this.bookingService.updateBooking(id, updatedBooking);
      this.onCancel.emit({
        id: id,
        status: updatedBooking.status as BookingStatus,
      } as BookingStatusUpdate);

      this.messageService.add({
        severity: 'success',
        summary: 'Reopened',
        detail: `Booking #${id} was reopened successfully`,
      });
    } catch (error) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: `Failed to reopen booking #${id}`,
      });
    }
  }
}
