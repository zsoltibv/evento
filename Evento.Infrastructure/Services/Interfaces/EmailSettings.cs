namespace Evento.Infrastructure.Services.Interfaces;

public class EmailSettings
{
    public string SenderName { get; set; } = null!;
    public string SenderEmail { get; set; } = null!;
    public string BrevoApiKey { get; set; } = null!;
    public string BrevoSmtpUri { get; set; } = null!;
}