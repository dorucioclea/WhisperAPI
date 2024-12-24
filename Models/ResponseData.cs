using JetBrains.Annotations;

namespace WhisperAPI.Models;

/// <summary>
/// The base response data.
/// </summary>
/// <param name="start">Start time of the sentence.</param>
/// <param name="end">End time of the sentence.</param>
/// <param name="text">The literal text of the sentence.</param>
public sealed class ResponseData(double start, double end, string text)
{
    /// <summary>
    /// Start time of the sentence.
    /// </summary>
    [UsedImplicitly] 
    public double Start { get; init; } = start;

    /// <summary>
    /// End time of the sentence.
    /// </summary>
    [UsedImplicitly] 
    public double End { get; init; } = end;

    /// <summary>
    /// The literal text of the sentence.
    /// </summary>
    [UsedImplicitly] 
    public string Text { get; init; } = text;

    public ResponseData() : this(0, 0, string.Empty)
    {
        
    }
}