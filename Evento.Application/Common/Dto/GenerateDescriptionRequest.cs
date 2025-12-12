using System.Text.Json.Serialization;

namespace Evento.Application.Common.Dto;

public record GenerateDescriptionRequest(
    [property: JsonPropertyName("prompt")] string Prompt,
    [property: JsonPropertyName("max_tokens")] int MaxTokens = 128
);