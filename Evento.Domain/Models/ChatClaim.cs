namespace Evento.Domain.Models;

public class ChatClaim
{
    public int Id { get; set; }
    public required string UserId { get; set; }      
    public required string AgentId { get; set; }   
    public DateTime ClaimedAt { get; set; } = DateTime.UtcNow;
}