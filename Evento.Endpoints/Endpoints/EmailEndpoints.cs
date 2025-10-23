using Evento.Application.Common.Dto;
using Evento.Infrastructure.Services.Interfaces;

namespace Evento.Endpoints.Endpoints;

public static class EmailEndpoints
{
    public static WebApplication MapEmailEndpoints(this WebApplication app)
    {
        var emailGroup = app.MapGroup("/api/email");

        emailGroup.MapGet("/send", async (
                IEmailService emailService
            ) =>
            {
                var emailMessage = new EmailMessageDto
                (
                    "risos97156@nrlord.com",
                    "Test",
                    $"Test",
                    false
                );

                try
                {
                    await emailService.SendEmailAsync(emailMessage);
                    return Results.Ok("Email sent successfully with bookings info.");
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error sending email: {ex.Message}",
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        return app;
    }
}