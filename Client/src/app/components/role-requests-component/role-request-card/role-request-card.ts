import { AuthService } from './../../../services/auth-service';
import { Component, computed, inject, input, output, signal } from '@angular/core';
import { RoleRequest } from '../../../models/RoleRequest';
import { CardModule } from 'primeng/card';
import { CommonModule } from '@angular/common';
import { BookingDatePipe } from '../../../pipe/booking-date-pipe';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-role-request-card',
  imports: [CardModule, CommonModule, BookingDatePipe, ButtonModule],
  templateUrl: './role-request-card.html',
  styleUrl: './role-request-card.scss',
})
export class RoleRequestCard {
  roleRequest = input.required<RoleRequest>();
  onCancel = output<number>();

  private authService = inject(AuthService);

  protected readonly showApproveButton = computed(() => this.roleRequest().status === 'Pending');
  protected readonly showRejectButton = computed(() => this.roleRequest().status === 'Pending');
  protected readonly isAdmin = computed(() => this.authService.isAdmin());

  getStatusClass(status: string): string {
    switch (status) {
      case 'Pending':
        return 'bg-yellow-500';
      case 'Approved':
        return 'bg-green-500';
      case 'Rejected':
        return 'bg-red-500';
      default:
        return 'bg-gray-500';
    }
  }

  protected async approveRoleRequest(id: number) {}

  protected async rejectRoleRequest(id: number) {}
}
