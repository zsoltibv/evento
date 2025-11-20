namespace Evento.Application.Common.Dto;

public record BookingWithInfo(
    int Id,
    string UserId,
    DateTime StartDate,
    DateTime EndDate,
    DateTime BookingDate,
    string Status,
    int VenueId,
    string VenueName,
    bool IsPaid,
    decimal AmountPaid
);