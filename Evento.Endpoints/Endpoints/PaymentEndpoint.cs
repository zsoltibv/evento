using Evento.Application.Common.Dto;
using Evento.Payments.Services;

namespace Evento.Endpoints.Endpoints;

public static class PaymentEndpoint
{
    public static WebApplication MapPaymentEndpoints(this WebApplication app)
    {
        var paymentGroup = app.MapGroup("/api/payments");

        paymentGroup.MapPost("/create-checkout", async (
                CreateIntentRequest req,
                IPaymentService paymentService) =>
            {
                var clientSecret = await paymentService.CreateCheckoutSessionAsync(
                    req.CustomerId,
                    req.PricePerHour,
                    req.Minutes,
                    req.BookingId);

                return Results.Ok(new { clientSecret });
            })
            .RequireAuthorization();

        paymentGroup.MapGet("/session-status", async (string sessionId, IPaymentService paymentService) =>
            {
                var sessionStatus = await paymentService.GetStripeSessionStatusAsync(sessionId);
                return Results.Ok(sessionStatus);
            })
            .RequireAuthorization();

        return app;
    }
}