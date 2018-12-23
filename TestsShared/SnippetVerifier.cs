using System.Collections.Generic;
using System.IO;
using System.Text;
using CaptureSnippets;
using ObjectApproval;

internal static class SnippetVerifier
{
    public static void Verify(string markdownContent, IReadOnlyDictionary<string, IReadOnlyList<Snippet>> availableSnippets)
    {
        var markdownProcessor = new MarkdownProcessor(
            snippets: availableSnippets,
            appendSnippetGroup: SimpleSnippetMarkdownHandling.AppendGroup);
        var stringBuilder = new StringBuilder();
        using (var reader = new StringReader(markdownContent))
        using (var writer = new StringWriter(stringBuilder))
        {
            var processResult = markdownProcessor.Apply(reader, writer);
            var output = new
            {
                processResult.MissingSnippets,
                processResult.UsedSnippets,
                content = stringBuilder.ToString()
            };
            ObjectApprover.VerifyWithJson(output, s => s.Replace("\\r\\n", "\r\n"));
        }
    }
}