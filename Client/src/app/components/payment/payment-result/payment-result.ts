import { PaymentService } from './../../../services/payment-service';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { StripeSessionStatus } from '../../../models/StripeModels';

@Component({
  selector: 'app-payment-result',
  imports: [],
  templateUrl: './payment-result.html',
  styleUrl: './payment-result.scss',
})
export class PaymentResult {
  private route = inject(ActivatedRoute);
  private paymentService = inject(PaymentService);

  status = signal<StripeSessionStatus | null>(null);

  async ngOnInit() {
    const sessionId = this.route.snapshot.queryParamMap.get('sessionId');

    if (!sessionId) {
      this.status.set(null);
      return;
    }

    const result: StripeSessionStatus = await this.paymentService.getSessionStatus(sessionId);
    this.status.set(result);
  }
}
