using System.Text.Json.Serialization;

namespace Shared.Models.Responses;

public class ErrorBase
{
    protected ErrorBase()
    {
    }

    [JsonPropertyOrder(1)] public int StatusCode { get; protected set; }
    [JsonPropertyOrder(2)] public string Message { get; protected set; } = string.Empty;
    [JsonPropertyOrder(3)] public string RequestId { get; protected set; } = string.Empty;
    [JsonPropertyOrder(4)] public string Path { get; protected set; } = string.Empty;
    [JsonPropertyOrder(5)] public Dictionary<string, string>? Errors { get; protected set; }
}