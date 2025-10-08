using Evento.Domain;
using Evento.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Evento.Infrastructure.Repository;

public class BookingRepository(EventoDbContext db) : IBookingRepository
{
    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await db.Bookings
            .Include(b => b.User)
            .Include(b => b.Venue)
            .OrderByDescending(b => b.BookingDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetByUserAsync(string userId)
    {
        return await db.Bookings
            .Include(b => b.Venue)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BookingDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Booking?> GetByIdAsync(int id)
    {
        return await db.Bookings
            .Include(b => b.User)
            .Include(b => b.Venue)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Booking> CreateAsync(Booking booking)
    {
        db.Bookings.Add(booking);
        await db.SaveChangesAsync();
        return booking;
    }

    public async Task<Booking> UpdateAsync(Booking booking)
    {
        db.Update(booking);
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

    public IQueryable<Booking> GetAll() => db.Bookings;
    public IQueryable<Booking> GetByUser(string userId) => db.Bookings.Where(b => b.UserId == userId);
}