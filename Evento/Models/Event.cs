namespace Evento.Models;

public class Event
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    
    public int VenueId { get; set; }
    public Venue Venue { get; set; } = null!;

    public ICollection<Booking> Bookings { get; set; } = null!;
}