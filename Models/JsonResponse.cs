using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace WhisperAPI.Models;

/// <summary>
/// Represents a JSON response.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public sealed class JsonResponse
{
    /// <summary>
    /// Array of response data.
    /// </summary>
    /// <value>
    /// The list of response data.
    /// </value>
    [JsonPropertyName("data")]
    public List<ResponseData> Data { get; init; } = [];

    /// <summary>
    /// Count of sentences in the response.
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; set; }
}