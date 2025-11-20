export interface PaymentResponse {
  clientSecret: string;
}

export interface PaymentIntent {
  customerId: string;
  pricePerHour: number;
  minutes: number;
  bookingId: number;
}

export interface StripeSessionStatus {
  status: string;
  email: string;
  amountPaid: number;
  bookingId: number;
}
