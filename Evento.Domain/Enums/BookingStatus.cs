using static System.Enum;

namespace Evento.Domain.Enums;

public enum BookingStatus
{
    Pending = 0,
    Approved = 1,
    Cancelled = 2
}

public static class BookingStatusExtensions
{
    public static bool TryToBookingStatus(this string? value, out BookingStatus status)
    {
        status = default;
        return !string.IsNullOrWhiteSpace(value) &&
               TryParse(value, out status);
    }

    public static bool EqualsStatus(this string? value, BookingStatus status)
        => string.Equals(value, status.ToString());

    public static BookingStatus ToBookingStatus(this string value)
        => Parse<BookingStatus>(value, ignoreCase: true);
}