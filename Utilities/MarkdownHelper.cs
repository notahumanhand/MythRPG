using Markdig;
using Microsoft.AspNetCore.Components;

namespace MythRPG.Utilities;

public static class MarkdownHelper
{
    private static readonly MarkdownPipeline _pipeline =
        new MarkdownPipelineBuilder()
            .UseSoftlineBreakAsHardlineBreak()
            .Build();

    public static MarkupString ToMarkup(string? markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
            return new MarkupString(string.Empty);

        string html = Markdown.ToHtml(markdown, _pipeline);

        return new MarkupString(html);
    }
}