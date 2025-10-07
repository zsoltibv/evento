namespace Evento.Domain.Models;

public class Venue
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public required string Location { get; set; }
    public int Capacity { get; set; }
    
    public ICollection<AppUser> VenueAdmins { get; set; } = new List<AppUser>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}