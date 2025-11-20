using Evento.Domain;
using Evento.Domain.Enums;
using Quartz;

namespace Evento.Jobs;

public class UnpaidBookingResetJob(IBookingRepository bookingRepository) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"[Quartz] Running unpaid booking check: {DateTime.Now}");

        var bookings = (await bookingRepository.GetUnpaidApprovedBookingsAsync()).ToList();
        
        if (bookings.Count == 0)
        {
            Console.WriteLine("[Quartz] No unpaid approved bookings found.");
            return;
        }

        foreach (var booking in bookings)
        {
            booking.Status = BookingStatus.Pending;
        }

        await bookingRepository.SaveChangesAsync();

        Console.WriteLine($"[Quartz] Reset {bookings.Count} unpaid bookings to Pending.");
    }
}