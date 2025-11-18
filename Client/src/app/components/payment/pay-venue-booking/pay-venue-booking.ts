import { AuthService } from './../../../services/auth-service';
import { UserService } from './../../../services/user-service';
import { Component, inject, OnInit, signal } from '@angular/core';
import { PaymentService } from '../../../services/payment-service';
import { PaymentIntent } from '../../../models/StripeModels';
import { loadStripe, Stripe } from '@stripe/stripe-js';
import { ActivatedRoute } from '@angular/router';
import { BookingService } from '../../../services/booking-service';

@Component({
  selector: 'app-pay-venue-booking',
  imports: [],
  templateUrl: './pay-venue-booking.html',
  styleUrl: './pay-venue-booking.scss',
})
export class PayVenueBooking implements OnInit {
  private paymentService = inject(PaymentService);
  private userService = inject(UserService);
  private authService = inject(AuthService);
  private route = inject(ActivatedRoute);
  private bookingService = inject(BookingService);

  private stripe: Stripe | null = null;
  private checkoutInitialized = false;
  loading = signal(false);
  customerId = signal<string | null>(null);
  bookingId = signal<number | null>(null);

  async ngOnInit() {
    this.loading.set(true);

    const idFromRoute = this.route.snapshot.paramMap.get('id');
    if (!idFromRoute) {
      console.error('No booking ID in route');
      this.loading.set(false);
      return;
    }
    this.bookingId.set(+idFromRoute);

    const id = await this.userService.getStripeCustomerId(this.authService.userId()!);
    this.customerId.set(id);

    this.stripe = await loadStripe(
      'pk_test_51SSuvFLQoUi2bB9tHkCnD7R3rvwH1u5LPkPx6MePTuEji3gwW8EUBRzsFVNkGglSuNOEBIwrtA981mtsiSgvOLQM00uHRFxqYF'
    );

    await this.initStripePayment();
  }

  protected async initStripePayment() {
    if (this.checkoutInitialized) return;
    this.checkoutInitialized = true;

    if (!this.stripe) {
      console.error('Stripe failed to load');
      this.loading.set(false);
      return;
    }

    if (!this.customerId()) {
      console.error('No Stripe Customer ID found');
      this.loading.set(false);
      return;
    }

    const booking = await this.bookingService.getBooking(this.bookingId()!);
    const start = new Date(booking.startDate);
    const end = new Date(booking.endDate);
    const diffMinutes = Math.ceil((end.getTime() - start.getTime()) / 60000);

    const pricePerHour = booking.venue.pricePerHour;
    const intent: PaymentIntent = {
      customerId: this.customerId()!,
      pricePerHour,
      minutes: diffMinutes,
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
