using System.Text;
using System.Text.Json;
using Evento.Application.Common.Dto;

namespace Evento.Endpoints.Endpoints;

public static class GenerateEndpoint
{
    public static WebApplication MapGenerateEndpoints(this WebApplication app)
    {
        var generateGroup = app.MapGroup("/api/generate");

        generateGroup.MapPost("/description",
                async (GenerateDescriptionRequest req, IHttpClientFactory factory) =>
                {
                    var client = factory.CreateClient("AiGenerator");

                    var json = JsonSerializer.Serialize(req);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("/generate", content);

                    if (!response.IsSuccessStatusCode)
                        return Results.Problem("AI service error", statusCode: 500);

                    var resultJson = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<GenerateDescriptionResponse>(resultJson);

                    return Results.Ok(result);
                })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces(StatusCodes.Status401Unauthorized);
        
        return app;
    }
}