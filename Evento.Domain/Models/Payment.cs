using Evento.Domain.Enums;

namespace Evento.Domain.Models;

public class Payment
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentTransactionId { get; set; } = null!;
    public PaymentStatus Status { get; set; } = PaymentStatus.Succeeded;
}