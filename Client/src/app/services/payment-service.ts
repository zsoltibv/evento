import { inject, Injectable } from '@angular/core';
import { RestApiService } from './rest-api-service';
import { PaymentIntent, PaymentResponse, StripeSessionStatus } from '../models/StripeModels';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {
  private api = inject(RestApiService);

  async createCheckoutSession(intent: PaymentIntent): Promise<PaymentResponse> {
    return await this.api.post<PaymentResponse>('/api/payments/create-checkout', intent);
  }

  async getSessionStatus(sessionId: string): Promise<StripeSessionStatus> {
    return await this.api.get<StripeSessionStatus>(
      `/api/payments/session-status?sessionId=${sessionId}`
    );
  }
}
