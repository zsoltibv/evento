export interface PaymentResponse {
  clientSecret: string;
}

export interface PaymentIntent {
  customerId: string;
  pricePerHour: number;
  minutes: number;
}

export interface StripeSessionStatus {
  status: string;
  email: string;
}
