using Evento.Domain.Models;

namespace Evento.Application.Common.Dto;

public record RoleRequestDto
{
    public int Id { get; set; }
    public required string RoleName { get; set; }
    public string Status { get; set; }
    public DateTime RequestDate { get; set; } 
    
    
    public AppUser? User { get; set; }
    
    public int? VenueId { get; set; }
    public Venue? Venue { get; set; }
}