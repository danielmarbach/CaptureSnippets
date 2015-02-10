using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaptureSnippets
{
    public class MarkdownProcessor
    {
        public ProcessResult ApplyToFile(List<SnippetGroup> snippets, string inputFile)
        {
            using (var reader = IndexReader.FromFile(inputFile))
            {
                return Apply(snippets, reader);
            }
        }

        public ProcessResult ApplyToText(List<SnippetGroup> availableSnippets, string markdownContent)
        {
            using (var reader = IndexReader.FromString(markdownContent))
            {
                return Apply(availableSnippets, reader);
            }
        }

        public ProcessResult Apply(List<SnippetGroup> availableSnippets, IndexReader reader)
        {
            var stringBuilder = new StringBuilder();
            var result = new ProcessResult();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                stringBuilder.AppendLine(line);

                string key;
                if (!ImportKeyReader.TryExtractKeyFromLine(line, out key))
                {
                    continue;
                }

                var snippetGroup = availableSnippets.FirstOrDefault(x=>x.Key == key);
                if (snippetGroup == null)
                {
                    var missingSnippet = new MissingSnippet
                    {
                        Key = key,
                        Line = reader.Index
                    };
                    result.MissingSnippet.Add(missingSnippet);
                    stringBuilder.AppendLine(string.Format("** Could not find key '{0}' **", key));
                    continue;
                }

                AppendGroup(snippetGroup, stringBuilder);
                if (result.UsedSnippets.All(x => x.Key != snippetGroup.Key))
                {
                    result.UsedSnippets.Add(snippetGroup);
                }
            }
            result.Text = stringBuilder.ToString().TrimTrailingNewLine();
            return result;
        }

        public void AppendGroup(SnippetGroup snippetGroup, StringBuilder stringBuilder)
        {
            foreach (var versionGroup in snippetGroup)
            {
                if (versionGroup.Version != null)
                {
                    stringBuilder.AppendLine("#### Version " + versionGroup.Version);
                }
                foreach (var snippet in versionGroup)
                {
                    AppendSnippet(snippet, stringBuilder);
                }
            }
        }

        public void AppendSnippet(Snippet codeSnippet, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("```" + codeSnippet.Language);
            stringBuilder.AppendLine(codeSnippet.Value);
            stringBuilder.AppendLine("```");
        }
    }
}