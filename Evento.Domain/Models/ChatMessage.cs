namespace Evento.Domain.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public required string MessageText { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    public required string SenderId { get; set; }
    public AppUser? Sender { get; set; }

    public required string ReceiverId { get; set; }
    public AppUser? Receiver { get; set; }
}