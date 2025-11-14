using Evento.Application.Common.Dto;
using Evento.Payments.Services;

namespace Evento.Endpoints.Endpoints;

public static class PaymentEndpoint
{
    public static WebApplication MapPaymentEndpoints(this WebApplication app)
    {
        var paymentGroup = app.MapGroup("/api/payments");

        paymentGroup.MapPost("/create-intent", async (
                CreateIntentRequest req,
                IPaymentService paymentService) =>
            {
                var clientSecret = await paymentService.CreateVenuePaymentIntentAsync(
                    req.CustomerId,
                    req.PricePerHour,
                    req.Hours);

                return Results.Ok(new { clientSecret });
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        return app;
    }
}