import { AuthService } from './../../services/auth-service';
import { Component, computed, effect, inject, input, output, signal } from '@angular/core';
import { BookingDatePipe } from '../../pipe/booking-date-pipe';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BookingService } from '../../services/booking-service';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { BookingStatus } from '../enums/BookingStatus';
import { UpdateBooking } from '../../models/UpdateBooking';
import { BookingStatusUpdate } from '../../models/BookingStatusUpdate';
import { BookingWithVenueName } from '../../models/BookingWithVenueName';
import { CommonModule } from '@angular/common';
import { DialogModule } from 'primeng/dialog';
import { AddOrEditBookingCard } from '../add-or-edit-booking-card/add-or-edit-booking-card';
import { Router } from '@angular/router';

@Component({
  selector: 'app-booking-card',
  imports: [
    BookingDatePipe,
    CardModule,
    ButtonModule,
    ConfirmDialogModule,
    CommonModule,
    DialogModule,
    AddOrEditBookingCard,
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
  private router = inject(Router);

  currentBooking = signal<BookingWithVenueName | null>(null);

  constructor() {
    effect(() => {
      const b = this.booking();
      if (b) {
        this.currentBooking.set(structuredClone(b));
      }
    });
  }

  protected readonly showEditDialog = signal(false);
  protected toggleEditDialog() {
    this.showEditDialog.set(!this.showEditDialog());
  }

  protected readonly isUsersBooking = computed(
    () => this.authService.userId() == this.currentBooking()?.userId
  );

  protected readonly showCancelButton = computed(
    () =>
      this.authService.isUser() &&
      this.currentBooking()?.status === BookingStatus.Pending &&
      this.isUsersBooking()
  );

  protected readonly showReopenButton = computed(
    () =>
      this.authService.isUser() &&
      this.currentBooking()?.status === BookingStatus.Cancelled &&
      this.isUsersBooking()
  );

  protected readonly showApproveButton = computed(
    () => this.currentBooking()?.status === BookingStatus.Pending && !this.isUsersBooking()
  );

  protected readonly showRejectButton = computed(
    () => this.currentBooking()?.status === BookingStatus.Pending && !this.isUsersBooking()
  );

  protected readonly showEditButton = computed(
    () =>
      this.authService.isUser() &&
      this.currentBooking()?.status === BookingStatus.Pending &&
      this.isUsersBooking()
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

  protected onBookingEdited(updatedBooking: BookingWithVenueName) {
    this.currentBooking.set(updatedBooking);
    this.showEditDialog.set(false);
  }

  protected goToBooking() {
    this.router.navigate(['/bookings', this.booking().id]);
  }
}
