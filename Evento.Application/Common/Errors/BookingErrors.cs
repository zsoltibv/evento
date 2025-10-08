namespace Evento.Application.Common.Errors;

public static class BookingErrors
{
    public static readonly Error StartDateRequired =
        new("BookingErrors.StartDateRequired", "Start date is required.");

    public static readonly Error StartDateBeforeEndDate =
        new("BookingErrors.StartDateBeforeEndDate", "Start date must be before end date.");

    public static readonly Error StartDateInFuture =
        new("BookingErrors.StartDateInFuture", "Start date must be in the future.");

    public static readonly Error EndDateRequired =
        new("BookingErrors.EndDateRequired", "End date is required.");

    public static readonly Error EndDateAfterStartDate =
        new("BookingErrors.EndDateAfterStartDate", "End date must be after start date.");

    public static readonly Error EndDateInFuture =
        new("BookingErrors.EndDateInFuture", "End date must be in the future.");

    public static readonly Error VenueRequired =
        new("BookingErrors.VenueRequired", "Venue id is required.");

    public static readonly Error VenueNotFound =
        new("BookingErrors.VenueNotFound", "Venue doesn't exist.");

    public static readonly Error StatusRequired =
        new("BookingErrors.StatusRequired", "Status is required.");

    public static readonly Error InvalidStatus =
        new("BookingErrors.InvalidStatus", "Invalid booking status.");

    public static readonly Error OverlappingAnyApprovedBooking =
        new("BookingErrors.OverlappingAnyApprovedBooking", "Booking is overlapping with an existing approved booking.");

    public static readonly Error OverlappingUserApprovedOrPendingBooking =
        new("BookingErrors.OverlappingUserApprovedOrPendingBooking",
            "Booking is overlapping with an existing approved and/or pending booking.");

    public static readonly Error UserCannotApproveOrRejectBooking =
        new("BookingErrors.UserCannotApproveOrRejectBooking", "User cannot approve/reject booking.");
}