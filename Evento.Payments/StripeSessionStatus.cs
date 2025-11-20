namespace Evento.Payments;

public record StripeSessionStatus(
    string Status,
    string Email,
    decimal AmountPaid,
    int BookingId
);