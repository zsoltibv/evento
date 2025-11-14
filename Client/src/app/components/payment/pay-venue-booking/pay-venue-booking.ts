import { Component, inject, signal } from '@angular/core';
import { PaymentService } from '../../../services/payment-service';
import { PaymentIntent } from '../../../models/StripeModels';
import { loadStripe, Stripe } from '@stripe/stripe-js';

@Component({
  selector: 'app-pay-venue-booking',
  imports: [],
  templateUrl: './pay-venue-booking.html',
  styleUrl: './pay-venue-booking.scss',
})
export class PayVenueBooking {
  private paymentService = inject(PaymentService);

  private stripe: Stripe | null = null;
  loading = signal(false);

  async ngAfterViewInit() {
    this.loading.set(true);

    this.stripe = await loadStripe(
      'pk_test_51SSuvFLQoUi2bB9tHkCnD7R3rvwH1u5LPkPx6MePTuEji3gwW8EUBRzsFVNkGglSuNOEBIwrtA981mtsiSgvOLQM00uHRFxqYF'
    );

    if (!this.stripe) {
      console.error('Failed to load Stripe');
      this.loading.set(false);
      return;
    }

    const intent: PaymentIntent = {
      customerId: 'cus_TPtL27WGw3nOgO',
      pricePerHour: 2,
      hours: 1,
    };

    const response = await this.paymentService.createCheckoutSession(intent);
    const clientSecret = response.clientSecret;

    const checkout = await this.stripe.initEmbeddedCheckout({
      fetchClientSecret: async () => clientSecret,
    });

    checkout.mount('#checkout');
    this.loading.set(false);
  }
}
