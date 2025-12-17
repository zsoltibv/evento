namespace Evento.Application.Common.Dto;

public class BookingFilter
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Status { get; set; }
    public int? VenueId { get; set; }
    public bool? IsPaid { get; set; }
}