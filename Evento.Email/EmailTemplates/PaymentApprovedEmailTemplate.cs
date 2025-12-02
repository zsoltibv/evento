using Evento.Email.Services.Interfaces;

namespace Evento.Email.EmailTemplates;

public class PaymentApprovedEmailTemplate : IEmailTemplate
{
    public EmailMessageDto Create(string to, object? data = null)
    {
        var bookingInfo = data as string ?? "your payment";

        var subject = $"Payment Approved for {bookingInfo}";
        var body = $"""
                    Hello,
                    <br/><br/>
                    Your payment for <strong>{bookingInfo}</strong> has been successfully approved.
                    You can now proceed with your booking details or further actions.
                    <br/><br/>
                    Thank you for choosing Evento!
                    <br/><br/>
                    Best regards,<br/>
                    Evento Team
                    """;

        return new EmailMessageDto(to, subject, body);
    }
}