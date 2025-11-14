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
                    req.Hours);

                return Results.Ok(new { clientSecret });
            })
            .RequireAuthorization();
        
        return app;
    }
}