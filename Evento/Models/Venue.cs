namespace Evento.Models;

public class Venue
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Location { get; set; }
    public int Capacity { get; set; }

    public ICollection<Event> Events { get; } = null!;
}