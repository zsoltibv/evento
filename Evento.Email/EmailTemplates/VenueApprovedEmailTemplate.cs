using Evento.Email.Services.Interfaces;

namespace Evento.Email.EmailTemplates;

public class VenueAdminApprovedEmailTemplate : IEmailTemplate
{
    public EmailMessageDto Create(string to, object? data = null)
    {
        var venueName = data as string ?? "your venue";

        var subject = $"Admin Access Approved for {venueName}";
        var body = $"""
                    Hello,
                    <br/><br/>
                    Your admin access for <strong>{venueName}</strong> has been approved.
                    You can now manage your venue.
                    <br/><br/>
                    Best regards,<br/>
                    Evento Team
                    """;

        return new EmailMessageDto(to, subject, body);
    }
}