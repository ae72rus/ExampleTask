using System.Text.Json.Serialization;

namespace Zvonarev.FinBeat.Test.WebApi.Models;

public record ErrorResponse
{
    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; init; }
}