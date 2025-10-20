using Evento.Domain.Enums;
using Evento.Domain.Models;

namespace Evento.Application.Common.Dto;

public record RoleRequestDto
{
    public required string RoleName { get; set; }
    public RequestStatus Status { get; set; }
    public DateTime RequestDate { get; set; } 
    
    public int? VenueId { get; set; }
    public Venue? Venue { get; set; }
}