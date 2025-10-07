import { Component, inject, input, output } from '@angular/core';
import { BookingDatePipe } from '../../pipe/booking-date-pipe';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Booking } from '../../models/Booking';
import { BookingService } from '../../services/booking-service';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';

@Component({
  selector: 'app-booking-card',
  imports: [BookingDatePipe, CardModule, ButtonModule, ConfirmDialogModule],
  templateUrl: './booking-card.html',
  styleUrl: './booking-card.scss',
})
export class BookingCard {
  booking = input.required<Booking>();
  onDelete = output<number>();

  private messageService = inject(MessageService);
  private bookingService = inject(BookingService);
  private confirmationService = inject(ConfirmationService);

  protected confirmDeleteBooking(id: number, event: any) {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to delete this booking?',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      rejectLabel: 'Cancel',
      accept: async () => await this.deleteBooking(id),
    });
  }

  async deleteBooking(id: number) {
    try {
      await this.bookingService.deleteBooking(id);
      this.onDelete.emit(id);

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
    }
  }
}
