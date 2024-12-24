using WhisperAPI.Utils;

namespace WhisperAPI.Extensions;

internal static class ResultsExtensions
{
    public static IResult Html(this IResultExtensions resultExtensions, string html)
    {
        ArgumentNullException.ThrowIfNull(resultExtensions);
        return new HtmlResult(html);
    }
}