using Evento.Models;
using Microsoft.EntityFrameworkCore;

namespace Evento.Repository;

public class BookingRepository(EventoDbContext db) : IBookingRepository
{
    public async Task<List<Booking>> GetAllAsync()
    {
        return await db.Bookings
            .Include(b => b.User)
            .Include(b => b.Venue)
            .ToListAsync();
    }

    public async Task<List<Booking>> GetByUserAsync(string userId)
    {
        return await db.Bookings
            .Include(b => b.Venue)
            .Where(b => b.UserId == userId)
            .ToListAsync();
    }

    public async Task<Booking?> GetByIdAsync(int id)
    {
        return await db.Bookings
            .Include(b => b.User)
            .Include(b => b.Venue)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Booking> CreateAsync(Booking booking)
    {
        db.Bookings.Add(booking);
        await db.SaveChangesAsync();
        return booking;
    }

    public async Task<Booking?> UpdateAsync(int id, Booking updatedBooking)
    {
        var booking = await db.Bookings.FindAsync(id);
        if (booking == null) return null;

        booking.StartDate = updatedBooking.StartDate;
        booking.EndDate = updatedBooking.EndDate;
        booking.Status = updatedBooking.Status;
        booking.VenueId = updatedBooking.VenueId;

        await db.SaveChangesAsync();
        return booking;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var booking = await db.Bookings.FindAsync(id);
        if (booking == null) return false;

        db.Bookings.Remove(booking);
        await db.SaveChangesAsync();
        return true;
    }
}