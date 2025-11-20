import { PaymentService } from './../../../services/payment-service';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { StripeSessionStatus } from '../../../models/StripeModels';
import { BookingService } from '../../../services/booking-service';
import { UpdateBookingPayment } from '../../../models/UpdateBookingPayment';

@Component({
  selector: 'app-payment-result',
  imports: [],
  templateUrl: './payment-result.html',
  styleUrl: './payment-result.scss',
})
export class PaymentResult {
  private route = inject(ActivatedRoute);
  private paymentService = inject(PaymentService);
  private bookingService = inject(BookingService);

  status = signal<StripeSessionStatus | null>(null);

  async ngOnInit() {
    const sessionId = this.route.snapshot.queryParamMap.get('sessionId');

    if (!sessionId) {
      this.status.set(null);
      return;
    }

    const result: StripeSessionStatus = await this.paymentService.getSessionStatus(sessionId);
    console.log(result);
    if (result.status == 'complete') {
      await this.bookingService.updateBookingPayment(result.bookingId, {
        isPaid: true,
        amountPaid: result.amountPaid,
      } as UpdateBookingPayment);
    }

    this.status.set(result);
  }
}
