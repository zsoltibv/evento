import { AuthService } from './../../services/auth-service';
import { Component, computed, inject, input, output, signal } from '@angular/core';
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
import { ChipModule } from 'primeng/chip';
import { CommonModule } from '@angular/common';
import { DialogModule } from 'primeng/dialog';
import { AddBookingCard } from '../add-booking-card/add-booking-card';

@Component({
  selector: 'app-booking-card',
  imports: [
    BookingDatePipe,
    CardModule,
    ButtonModule,
    ConfirmDialogModule,
    ChipModule,
    CommonModule,
    DialogModule,
    AddBookingCard,
  ],
  templateUrl: './booking-card.html',
  styleUrl: './booking-card.scss',
})
export class BookingCard {
  booking = input.required<BookingWithVenueName>();
  onCancel = output<BookingStatusUpdate>();

  private messageService = inject(MessageService);
  private bookingService = inject(BookingService);
  private confirmationService = inject(ConfirmationService);
  public authService = inject(AuthService);

  protected readonly showEditDialog = signal(false);
  protected toggleEditDialog() {
    this.showEditDialog.set(!this.showEditDialog());
  }

  protected readonly showCancelButton = computed(
    () => this.authService.isUser() && this.booking()?.status === BookingStatus.Pending
  );

  protected readonly showReopenButton = computed(
    () => this.authService.isUser() && this.booking()?.status === BookingStatus.Cancelled
  );

  protected readonly showApproveButton = computed(
    () => this.authService.isAdmin() && this.booking()?.status === BookingStatus.Pending
  );

  protected readonly showRejectButton = computed(
    () => this.authService.isAdmin() && this.booking()?.status === BookingStatus.Pending
  );

  protected readonly showEditButton = computed(
    () => this.authService.isUser() && this.booking()?.status === BookingStatus.Pending
  );

  getStatusClass(status: BookingStatus): string {
    switch (status) {
      case BookingStatus.Pending:
        return 'bg-yellow-500';
      case BookingStatus.Approved:
        return 'bg-green-500';
      case BookingStatus.Cancelled:
        return 'bg-red-500';
      case BookingStatus.Rejected:
        return 'bg-red-500';
      default:
        return 'bg-gray-500';
    }
  }

  protected confirmCancelBooking(id: number) {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to cancel this booking?',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      rejectLabel: 'Cancel',
      accept: async () => await this.cancelBooking(id),
      acceptButtonStyleClass: 'p-button-danger',
      rejectButtonStyleClass: 'p-button-secondary p-button-outlined',
    });
  }

  async cancelBooking(id: number) {
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
      summary: 'Cancelled',
      detail: `Booking #${id} was cancelled successfully`,
    });
  }

  async reopenBooking(id: number) {
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
  }

  async approveBooking(id: number) {
    const updatedBooking: Partial<UpdateBooking> = {
      status: BookingStatus.Approved,
    };

    await this.bookingService.updateBooking(id, updatedBooking);
    this.onCancel.emit({
      id: id,
      status: updatedBooking.status as BookingStatus,
    } as BookingStatusUpdate);

    this.messageService.add({
      severity: 'success',
      summary: 'Approved',
      detail: `Booking #${id} was approved successfully`,
    });
  }

  async rejectBooking(id: number) {
    const updatedBooking: Partial<UpdateBooking> = {
      status: BookingStatus.Rejected,
    };

    await this.bookingService.updateBooking(id, updatedBooking);
    this.onCancel.emit({
      id: id,
      status: updatedBooking.status as BookingStatus,
    } as BookingStatusUpdate);

    this.messageService.add({
      severity: 'success',
      summary: 'Rejected',
      detail: `Booking #${id} was rejected successfully`,
    });
  }
}
