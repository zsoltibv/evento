namespace Evento.Domain.Models;

public class Venue
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public required string Location { get; set; }
    public int Capacity { get; set; }
    public required string Slug { get; set; }
    public decimal PricePerHour { get; set; } = 100;
    
    public ICollection<VenueAdmin> VenueAdmins { get; set; } = new List<VenueAdmin>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}