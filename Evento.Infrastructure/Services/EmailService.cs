using System.Text;
using Evento.Application.Common.Dto;
using Evento.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace Evento.Infrastructure.Services;

public class EmailService(IOptions<EmailSettings> options) : IEmailService
{
    private readonly EmailSettings _settings = options.Value;

    public async Task SendEmailAsync(EmailMessageDto message)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_settings.BrevoSmtpUri),
            Headers =
            {
                { "accept", "application/json" },
                { "api-key", _settings.BrevoApiKey },
            },
            Content = new StringContent(
                JsonConvert.SerializeObject(new
                {
                    sender = new { email = _settings.SenderEmail, name = _settings.SenderName },
                    to = new[] { new { email = message.To } },
                    subject = message.Subject,
                    htmlContent = message.Body,
                    textContent = message.Body
                }),
                Encoding.UTF8,
                "application/json"
            )
        };

        try
        {
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Email sent successfully: {body}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            throw;
        }
    }
}