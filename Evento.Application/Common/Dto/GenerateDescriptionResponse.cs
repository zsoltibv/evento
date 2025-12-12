using System.Text.Json.Serialization;

namespace Evento.Application.Common.Dto;

public record GenerateDescriptionResponse(
    [property: JsonPropertyName("response")] string Response
);
