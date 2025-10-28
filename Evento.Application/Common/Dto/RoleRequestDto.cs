namespace Evento.Application.Common.Dto;

public record RoleRequestDto
{
    public int Id { get; set; }
    public required string RoleName { get; set; }
    public string Status { get; set; }
    public DateTime RequestDate { get; set; }
    
    public UserDto? User { get; set; }

    public int? VenueId { get; set; }
    public VenueDto? Venue { get; set; }
}