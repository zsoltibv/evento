import { inject, Injectable } from '@angular/core';
import { RestApiService } from './rest-api-service';
import { PaymentIntent, PaymentResponse } from '../models/StripeModels';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {
  private api = inject(RestApiService);

  async createCheckoutSession(intent: PaymentIntent): Promise<PaymentResponse> {
    return await this.api.post<PaymentResponse>('/api/payments/create-checkout', intent);
  }
}
