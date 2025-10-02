namespace Evento.Domain.Models;

public class Venue
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public byte[]? Image { get; set; }
    public required string Location { get; set; }
    public int Capacity { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}