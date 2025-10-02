namespace Evento.Application.Common.Errors;

public static class BookingErrors
{
    public static readonly ErrorResponse StartDateRequired =
        new("BookingErrors.StartDateRequired", "Start date is required.");

    public static readonly ErrorResponse StartDateBeforeEndDate =
        new("BookingErrors.StartDateBeforeEndDate", "Start date must be before end date.");

    public static readonly ErrorResponse StartDateInFuture =
        new("BookingErrors.StartDateInFuture", "Start date must be in the future.");

    public static readonly ErrorResponse EndDateRequired =
        new("BookingErrors.EndDateRequired", "End date is required.");

    public static readonly ErrorResponse EndDateAfterStartDate =
        new("BookingErrors.EndDateAfterStartDate", "End date must be after start date.");

    public static readonly ErrorResponse EndDateInFuture =
        new("BookingErrors.EndDateInFuture", "End date must be in the future.");

    public static readonly ErrorResponse VenueRequired =
        new("BookingErrors.VenueRequired", "Venue id is required.");

    public static readonly ErrorResponse VenueNotFound =
        new("BookingErrors.VenueNotFound", "Venue doesn't exist.");

    public static readonly ErrorResponse StatusRequired =
        new("BookingErrors.StatusRequired", "Status is required.");

    public static readonly ErrorResponse InvalidStatus =
        new("BookingErrors.InvalidStatus", "Invalid booking status.");

    public static readonly ErrorResponse OverlappingAnyApprovedBooking =
        new("BookingErrors.OverlappingAnyApprovedBooking", "Booking is overlapping with an existing approved booking.");

    public static readonly ErrorResponse OverlappingUserApprovedOrPendingBooking =
        new("BookingErrors.OverlappingUserApprovedOrPendingBooking",
            "Booking is overlapping with an existing approved and/or pending booking.");

    public static readonly ErrorResponse UserCannotApproveBooking =
        new("BookingErrors.UserCannotApproveBooking", "User cannot approve booking.");
}