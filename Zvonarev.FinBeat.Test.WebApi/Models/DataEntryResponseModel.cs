using System.Text.Json.Serialization;

namespace Zvonarev.FinBeat.Test.WebApi.Models;

public record DataEntryResponseModel
{
    [JsonPropertyName("orderId")]
    public int OrderId { get; init; }
    [JsonPropertyName("code")]
    public int Code { get; init; }
    [JsonPropertyName("value")]
    public string Value { get; init; }
}